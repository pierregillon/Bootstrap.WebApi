using Bootstrap.Domain.Customers;

namespace Bootstrap.Tests.Unit.Builders;

public class CustomerBuilder
{
    private Guid id = Guid.NewGuid();
    private string firstName = "John";
    private string lastName = "Doe";

    private Customer Build() => Customer.Rehydrate(id, firstName, lastName);

    public static implicit operator Customer(CustomerBuilder builder) => builder.Build();
}