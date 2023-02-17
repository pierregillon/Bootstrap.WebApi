namespace Bootstrap.BuildingBlocks.Aggregates;

public abstract class AggregateRoot<T>
{
    protected AggregateRoot(T id) => Id = id;

    public T Id { get; }
}
