using MediatR;
using Microsoft.Extensions.Logging;
using Shared.Core;

namespace Dal.Postgres;

internal sealed class UnitOfWorkPipeline<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
    where TResponse : notnull
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMediator _mediator;
    private readonly ILogger<UnitOfWorkPipeline<TRequest, TResponse>> _logger;

    public UnitOfWorkPipeline(IUnitOfWork unitOfWork, IMediator mediator, ILogger<UnitOfWorkPipeline<TRequest, TResponse>> logger)
    {
        _unitOfWork = unitOfWork;
        _mediator = mediator;
        _logger = logger;
    }
    
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var response = await next();
        
        if (typeof(TRequest).Name.EndsWith("Command",StringComparison.InvariantCultureIgnoreCase))
        {
            var domainEvents = _unitOfWork.GetChangeTracker().Entries<Entity>()
                .Select(x => x.Entity as Entity)
                .SelectMany(x => x.DomainEvents)
                .ToList();

            _logger.LogDebug("Publishing domain events ({domainEventsCount})...", domainEvents.Count);
            
            foreach (var @domainEvent in domainEvents)
            {
                await _mediator.Publish(@domainEvents, cancellationToken);
            }
            
            _logger.LogDebug("Saving changes...");
            
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            
            _logger.LogDebug("Saved changes.");
        }

        return response;

    }
}