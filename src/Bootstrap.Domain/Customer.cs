namespace Bootstrap.Domain;

public class Customer
{
    public static Customer Register(string firstName, string lastName) => new(Guid.NewGuid(), firstName, lastName);
    public static Customer Rehydrate(Guid id, string firstName, string lastName) => new(id, firstName, lastName);

    private Customer(Guid id, string firstName, string lastName)
    {
        this.Id = id;
        this.FirstName = firstName;
        this.LastName = lastName;
    }

    public Guid Id { get; }
    public string FirstName { get; private set; }
    public string LastName { get; private set; }

    public void Rename(string firstName, string lastName)
    {
        this.FirstName = firstName;
        this.LastName = lastName;
    }
}