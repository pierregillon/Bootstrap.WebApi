namespace Bootstrap.BuildingBlocks.DomainEvents;

public static class DomainEventPublisherExtensions
{
    public static async Task Publish(this IDomainEventPublisher domainEventPublisher, IEnumerable<IDomainEvent> domainEvents)
    {
        foreach (var domainEvent in domainEvents)
        {
            await domainEventPublisher.Publish(domainEvent);
        }
    }
}
