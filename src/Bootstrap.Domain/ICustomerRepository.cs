namespace Bootstrap.Domain;

public interface ICustomerRepository
{
    Task<Customer> Get(Guid customerId);
    Task Save(Customer customer);
}