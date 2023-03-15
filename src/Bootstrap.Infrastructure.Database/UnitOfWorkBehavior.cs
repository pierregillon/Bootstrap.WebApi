using System.Transactions;
using MediatR;

namespace Bootstrap.Infrastructure.Database;

internal class UnitOfWorkBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
{
    private readonly BootstrapDbContext _dbContext;

    public UnitOfWorkBehavior(BootstrapDbContext dbContext) => _dbContext = dbContext;

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        using var scope = BuildTransactionScope();

        var response = await next();

        await _dbContext.SaveChangesAsync(cancellationToken);

        scope.Complete();

        return response;
    }

    private static TransactionScope BuildTransactionScope() =>
        new(
            TransactionScopeOption.Required,
            new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted },
            TransactionScopeAsyncFlowOption.Enabled
        );
}
