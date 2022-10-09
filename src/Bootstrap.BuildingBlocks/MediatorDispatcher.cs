using Bootstrap.BuildingBlocks.Commands;
using Bootstrap.BuildingBlocks.Queries;
using MediatR;

namespace Bootstrap.BuildingBlocks;

internal class MediatorDispatcher : ICommandDispatcher, IQueryDispatcher
{
    private readonly IMediator _mediator;

    public MediatorDispatcher(IMediator mediator) => _mediator = mediator;

    public async Task Dispatch(ICommand command) => await _mediator.Send(command);

    public async Task<TResult> Dispatch<TResult>(ICommand<TResult> command) => await _mediator.Send(command);

    public async Task<TResult> Dispatch<TResult>(IQuery<TResult> query) => await _mediator.Send(query);
}
