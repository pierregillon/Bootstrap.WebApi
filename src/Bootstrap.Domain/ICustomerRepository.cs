namespace Bootstrap.Domain;

public interface ICustomerRepository
{
    Task Save(Customer customer);
}