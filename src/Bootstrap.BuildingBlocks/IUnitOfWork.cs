namespace Bootstrap.BuildingBlocks;

public interface IUnitOfWork
{
    Task ExecuteInTransaction(Func<Task> action);
}
