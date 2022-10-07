using Bootstrap.Domain;
using Microsoft.EntityFrameworkCore;

namespace Bootstrap.Infrastructure;

internal class SqlCustomerRepository : ICustomerRepository
{
    private readonly BootstrapDbContext _dbContext;

    public SqlCustomerRepository(BootstrapDbContext dbContext)
    {
        this._dbContext = dbContext;
    }

    public async Task<Customer> Get(Guid customerId)
    {
        var entity = await this._dbContext.Customers.SingleOrDefaultAsync(x => x.Id == customerId);
        if (entity is null)
        {
            throw new InvalidOperationException($"Cannot found the user with id {customerId}");
        }

        return Customer.Rehydrate(entity.Id, entity.FirstName, entity.LastName);
    }

    public async Task Save(Customer customer)
    {
        var entity = await this._dbContext.Customers.FindAsync(customer.Id);
        if (entity is null)
        {
            this._dbContext.Customers.Add(
                new CustomerEntity
                {
                    Id = customer.Id, FirstName = customer.FirstName, LastName = customer.LastName
                });
        }
        else
        {
            entity.FirstName = customer.FirstName;
            entity.LastName = customer.LastName;
        }

        await this._dbContext.SaveChangesAsync();
    }
}