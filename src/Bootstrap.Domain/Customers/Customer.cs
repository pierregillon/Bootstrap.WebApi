namespace Bootstrap.Domain.Customers;

public class Customer
{
    public static Customer Register(string firstName, string lastName) => new(Guid.NewGuid(), firstName, lastName);
    public static Customer Rehydrate(Guid id, string firstName, string lastName) => new(id, firstName, lastName);

    private Customer(Guid id, string firstName, string lastName)
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
    }

    public Guid Id { get; }
    public string FirstName { get; private set; }
    public string LastName { get; private set; }

    public void Rename(string firstName, string lastName)
    {
        if (string.IsNullOrWhiteSpace(firstName))
        {
            throw new ArgumentException("Value cannot be null or whitespace.", nameof(firstName));
        }
        if (string.IsNullOrWhiteSpace(lastName))
        {
            throw new ArgumentException("Value cannot be null or whitespace.", nameof(lastName));
        }
        FirstName = firstName;
        LastName = lastName;
    }
}
