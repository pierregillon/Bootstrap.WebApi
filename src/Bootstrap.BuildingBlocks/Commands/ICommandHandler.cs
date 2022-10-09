using MediatR;

namespace Bootstrap.BuildingBlocks.Commands;

public interface ICommandHandler<in T> : IRequestHandler<T, Unit> where T : ICommand
{
    async Task<Unit> IRequestHandler<T, Unit>.Handle(T request, CancellationToken _)
    {
        await Handle(request);
        return Unit.Value;
    }

    Task Handle(T command);
}

public interface ICommandHandler<in T, TResult> : IRequestHandler<T, TResult> where T : ICommand<TResult>
{
    async Task<TResult> IRequestHandler<T, TResult>.Handle(T request, CancellationToken _)
    {
        return await Handle(request);
    }

    Task<TResult> Handle(T command);
}
