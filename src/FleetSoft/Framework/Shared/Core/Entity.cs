namespace Shared.Core;

public abstract class Entity
{
    private readonly List<IDomainEvent> _domainEvents = [];
    public IReadOnlyList<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();
    public Id Id { get; protected set; }
    
    public void RaiseUp(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }
}