namespace Bootstrap.Domain.Users;

public class BadEmailAddressFormatException : Exception
{
    public BadEmailAddressFormatException() : base("The provided email has a bad format.")
    {
    }
}
