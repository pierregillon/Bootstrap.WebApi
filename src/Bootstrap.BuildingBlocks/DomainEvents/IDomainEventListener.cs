using System.Diagnostics;
using MediatR;

namespace Bootstrap.BuildingBlocks.DomainEvents;

public interface IDomainEventListener<in T> : INotificationHandler<T> where T : IDomainEvent
{
    Task INotificationHandler<T>.Handle(T request, CancellationToken _)
    {
        using Activity? activity = Activity.Current?.Source.StartActivity(GetType().Name);
        return On(request);
    }

    Task On(T domainEvent);
}
