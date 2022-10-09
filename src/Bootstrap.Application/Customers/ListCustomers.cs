using Bootstrap.BuildingBlocks.Queries;
using Bootstrap.Infrastructure;
using Bootstrap.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Bootstrap.Application.Customers;

public record ListCustomersQuery : IQuery<IReadOnlyCollection<CustomerListItemDto>>;

public record CustomerListItemDto(Guid Id, string FullName);

internal class ListCustomersHandler : IQueryHandler<ListCustomersQuery, IReadOnlyCollection<CustomerListItemDto>>
{
    private readonly BootstrapDbContext _dbContext;

    public ListCustomersHandler(BootstrapDbContext dbContext) => _dbContext = dbContext;

    public async Task<IReadOnlyCollection<CustomerListItemDto>> Handle(ListCustomersQuery command) =>
        await _dbContext.Customers
            .Select(x => new CustomerListItemDto(x.Id, x.FirstName + " " + x.LastName))
            .ToArrayAsync();
}
