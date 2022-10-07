using System.Transactions;
using Bootstrap.BuildingBlocks;
using Bootstrap.Infrastructure.EF;

namespace Bootstrap.Infrastructure;

public class UnitOfWork : IUnitOfWork
{
    private readonly BootstrapDbContext _dbContext;

    public UnitOfWork(BootstrapDbContext dbContext) => _dbContext = dbContext;

    public async Task ExecuteInTransaction(Func<Task> action)
    {
        using var scope = new TransactionScope();

        await action();

        await _dbContext.SaveChangesAsync();

        scope.Complete();
    }
}
