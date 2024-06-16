using MediatR;
using Shared.Core;

namespace Shared.Application.Events;

public abstract class ApplicationEventHandler<TDomainEvent> : INotificationHandler<ApplicationEvent<TDomainEvent>> where TDomainEvent : class, IDomainEvent
{
    public virtual Task Handle(ApplicationEvent<TDomainEvent> notification, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}