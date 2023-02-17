namespace Bootstrap.Domain.Users.Security;

public class TooWeakPasswordException : Exception
{
    public TooWeakPasswordException() : base("The provided password is too weak.")
    {
    }
}
