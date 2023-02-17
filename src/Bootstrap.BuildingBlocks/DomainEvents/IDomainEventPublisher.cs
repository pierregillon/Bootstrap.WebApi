namespace Bootstrap.BuildingBlocks.DomainEvents;

public interface IDomainEventPublisher
{
    Task Publish<T>(T domainEvent) where T : IDomainEvent;
}
