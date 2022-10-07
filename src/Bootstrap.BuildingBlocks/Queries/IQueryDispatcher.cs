namespace Bootstrap.BuildingBlocks.Queries;

public interface IQueryDispatcher
{
    Task<TResult> Dispatch<TResult>(IQuery<TResult> query);
}
