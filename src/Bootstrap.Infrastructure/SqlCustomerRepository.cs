using Bootstrap.Domain;

namespace Bootstrap.Infrastructure;

internal class SqlCustomerRepository : ICustomerRepository
{
    private readonly BootstrapDbContext _dbContext;

    public SqlCustomerRepository(BootstrapDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task Save(Customer customer)
    {
        _dbContext.Customers.Add(new CustomerEntity
        {
            Id = customer.Id,
            FirstName = customer.FirstName,
            LastName = customer.LastName
        });

        _dbContext.SaveChanges();

        return Task.CompletedTask;
    }
}