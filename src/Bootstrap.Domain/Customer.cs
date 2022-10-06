namespace Bootstrap.Domain;

public class Customer
{
    public static Customer Register(string firstName, string lastName)
    {
        return new Customer(Guid.NewGuid(), firstName, lastName);
    }

    private Customer(Guid id, string firstName, string lastName)
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
    }

    public Guid Id { get; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
}