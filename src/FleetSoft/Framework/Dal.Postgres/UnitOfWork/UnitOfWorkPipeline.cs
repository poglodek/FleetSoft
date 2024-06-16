using System.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using Shared.Core;

namespace Dal.Postgres.UnitOfWork;

internal sealed class UnitOfWorkPipeline<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
    where TResponse : notnull
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMediator _mediator;
    private readonly ILogger<UnitOfWorkPipeline<TRequest, TResponse>> _logger;

    internal UnitOfWorkPipeline(IUnitOfWork unitOfWork, IMediator mediator, ILogger<UnitOfWorkPipeline<TRequest, TResponse>> logger)
    {
        _unitOfWork = unitOfWork;
        _mediator = mediator;
        _logger = logger;
    }
    
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var isCommandRequest = typeof(TRequest).Name.EndsWith("Command", StringComparison.InvariantCultureIgnoreCase);
        
        if (!isCommandRequest)
        {
            return await next();
            
        }

        IDbContextTransaction transaction = null;
        
        try
        {
            transaction = await _unitOfWork.BeginTransactionAsync(cancellationToken);
            
            var response = await next();
                
            var domainEvents = ReturnDomainEvents();

            _logger.LogDebug("Publishing domain events ({domainEventsCount})...", domainEvents.Count);

            await PublishEvents(cancellationToken, domainEvents);

            _logger.LogDebug("Saving changes...");

            await SaveChangesAsync(cancellationToken, transaction);

            _logger.LogDebug("Saved changes.");

            return response;
        }
        catch (Exception e)
        {
            await transaction!.RollbackAsync(cancellationToken);
                
            _logger.LogError(e, "An error occurred while saving changes.");

            throw;
        }
        

    }

    private List<IDomainEvent> ReturnDomainEvents()
    {
        return _unitOfWork.GetChangeTracker().Entries<Entity>()
            .Select(x => x.Entity)
            .SelectMany(x => x.DomainEvents)
            .ToList();
    }

    private async Task PublishEvents(CancellationToken cancellationToken, List<IDomainEvent> domainEvents)
    {
        foreach (var @domainEvent in domainEvents)
        {
            await _mediator.Publish(@domainEvent, cancellationToken);
        }
    }

    private async Task SaveChangesAsync(CancellationToken cancellationToken, IDbContextTransaction? transaction)
    {
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        await transaction?.CommitAsync(cancellationToken)!;
    }
}