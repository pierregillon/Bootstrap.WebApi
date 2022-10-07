using Bootstrap.BuildingBlocks.Commands;
using Bootstrap.BuildingBlocks.Queries;
using MediatR;

namespace Bootstrap.BuildingBlocks;

internal class MediatorDispatcher : ICommandDispatcher, IQueryDispatcher
{
    private readonly IMediator _mediator;
    private readonly IUnitOfWork _unitOfWork;

    public MediatorDispatcher(IMediator mediator, IUnitOfWork unitOfWork)
    {
        _mediator = mediator;
        _unitOfWork = unitOfWork;
    }

    public async Task Dispatch(ICommand command)
    {
        await _unitOfWork.ExecuteInTransaction(async () =>
        {
            await _mediator.Send(command);
        });
    }

    public async Task<TResult> Dispatch<TResult>(IQuery<TResult> query) => await _mediator.Send(query);
}
