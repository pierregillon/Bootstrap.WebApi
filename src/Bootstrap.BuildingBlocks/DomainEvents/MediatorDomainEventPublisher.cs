using MediatR;

namespace Bootstrap.BuildingBlocks.DomainEvents;

public class MediatorDomainEventPublisher : IDomainEventPublisher
{
    private readonly IMediator _mediator;

    public MediatorDomainEventPublisher(IMediator mediator)
    {
        _mediator = mediator;
    }

    public Task Publish<T>(T domainEvent) where T : IDomainEvent
    {
        return _mediator.Publish(domainEvent);
    }
}
