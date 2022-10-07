using Bootstrap.BuildingBlocks.Queries;
using Bootstrap.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Bootstrap.Application;

public record ListCustomers : IQuery<IReadOnlyCollection<CustomerListItemDto>>;

public record CustomerListItemDto(Guid Id, string FullName);

internal class ListCustomersHandler : IQueryHandler<ListCustomers, IReadOnlyCollection<CustomerListItemDto>>
{
    private readonly BootstrapDbContext _dbContext;

    public ListCustomersHandler(BootstrapDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    async Task<IReadOnlyCollection<CustomerListItemDto>> IQueryHandler<ListCustomers, IReadOnlyCollection<CustomerListItemDto>>.Handle(ListCustomers command)
    {
        return await _dbContext.Customers
            .Select(x => new CustomerListItemDto(x.Id, x.FirstName + " " + x.LastName))
            .ToArrayAsync();
    }
}