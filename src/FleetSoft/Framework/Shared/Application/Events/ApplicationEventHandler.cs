using MediatR;
using Shared.Core;

namespace Shared.Application.Events;

public abstract class ApplicationEventHandler<TDomainEvent> : INotificationHandler<ApplicationEvent<TDomainEvent>> where TDomainEvent : IDomainEvent
{
    public virtual Task Handle(ApplicationEvent<TDomainEvent> notification, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}