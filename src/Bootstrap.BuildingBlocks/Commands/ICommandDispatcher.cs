namespace Bootstrap.BuildingBlocks.Commands;

public interface ICommandDispatcher
{
    Task Dispatch(ICommand command);
}