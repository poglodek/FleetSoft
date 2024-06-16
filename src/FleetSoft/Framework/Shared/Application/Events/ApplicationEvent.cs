using MediatR;
using Shared.Core;

namespace Shared.Application.Events;

public abstract class ApplicationEvent<TDomainEvent> : INotification where TDomainEvent: IDomainEvent
{
    protected TDomainEvent DomainEvent { get; }

    protected ApplicationEvent(TDomainEvent domainEvent)
    {
        DomainEvent = domainEvent;
    }
}