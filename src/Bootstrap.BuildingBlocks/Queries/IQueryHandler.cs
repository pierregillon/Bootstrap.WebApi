using MediatR;

namespace Bootstrap.BuildingBlocks.Queries;

public interface IQueryHandler<in T, TResult> : IRequestHandler<T, TResult> where T : IQuery<TResult>
{
    async Task<TResult> IRequestHandler<T, TResult>.Handle(T request, CancellationToken _)
    {
        return await Handle(request);
    }

    protected Task<TResult> Handle(T command);
}