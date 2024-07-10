using MediatR;
using Shared.Core;

namespace Shared.Application.Events;

public abstract class ApplicationEvent<TDomainEvent> : INotification where TDomainEvent: class, IDomainEvent
{
    protected TDomainEvent DomainEvent { get; }

    public ApplicationEvent(TDomainEvent domainEvent)
    {
        DomainEvent = domainEvent;
    }
}