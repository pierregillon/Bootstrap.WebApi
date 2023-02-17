using Bootstrap.BuildingBlocks.DomainEvents;

namespace Bootstrap.BuildingBlocks.Aggregates;

public abstract class EventDrivenAggregateRoot<T> : AggregateRoot<T>
{
    private readonly List<IDomainEvent> _uncommittedEvents = new();

    public IEnumerable<IDomainEvent> UncommittedEvents => _uncommittedEvents;

    protected EventDrivenAggregateRoot(T id) : base(id)
    {
    }

    protected void StoreEvent(IDomainEvent domainEvent)
    {
        this._uncommittedEvents.Add(domainEvent);
    }
}
