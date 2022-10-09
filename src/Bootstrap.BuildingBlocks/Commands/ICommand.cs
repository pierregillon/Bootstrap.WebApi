using MediatR;

namespace Bootstrap.BuildingBlocks.Commands;

public interface ICommand : IRequest
{
}

public interface ICommand<out TResult> : IRequest<TResult>
{
}
