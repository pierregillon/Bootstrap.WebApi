using MediatR;

namespace Bootstrap.BuildingBlocks.Queries;

public interface IQuery<out TResult> : IRequest<TResult>
{
}
