using System.Text.Json;
using MediatR;
using Shared.Core;

namespace Shared.Application.Events;

internal class DomainApplicationEventMapper<TDomainEvent>(IMediator mediator) : INotificationHandler<TDomainEvent>
    where TDomainEvent : IDomainEvent
{
    private readonly IMediator _mediator = mediator;

    public Task Handle(TDomainEvent domainEvent, CancellationToken cancellationToken)
    {
        var applicationEvent = (ApplicationEvent<TDomainEvent>)Activator.CreateInstance(typeof(ApplicationEvent<>).MakeGenericType(domainEvent.GetType()), domainEvent);
        
        if(applicationEvent is null)
        {
            throw new InvalidOperationException($"Could not create ApplicationEvent based on DomainEvent ({domainEvent.GetType().Name})");
        }
        
        return _mediator.Publish(applicationEvent, cancellationToken);
    }
}