namespace Bootstrap.BuildingBlocks.Commands;

public interface ICommandDispatcher
{
    Task Dispatch(ICommand command);
    Task<TResult> Dispatch<TResult>(ICommand<TResult> command);
}
