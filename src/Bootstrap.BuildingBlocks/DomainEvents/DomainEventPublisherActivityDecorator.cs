using System.Diagnostics;
using System.Text.Json;

namespace Bootstrap.BuildingBlocks.DomainEvents;

internal class DomainEventPublisherActivityDecorator : IDomainEventPublisher
{
    private readonly IDomainEventPublisher _decorated;

    public DomainEventPublisherActivityDecorator(IDomainEventPublisher decorated) => _decorated = decorated;

    public Task Publish<T>(T domainEvent) where T : IDomainEvent
    {
        using var activity = Activity.Current?.Source.StartActivity(domainEvent.GetType().Name);

        activity?.SetTag("json", JsonSerializer.Serialize(domainEvent));

        return _decorated.Publish(domainEvent);
    }
}
