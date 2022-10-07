namespace Bootstrap.Domain.Customers;

public interface ICustomerRepository
{
    Task<Customer> Get(Guid customerId);
    Task Save(Customer customer);
}
