namespace Bootstrap.Domain.Users;

public class EmailAddressAlreadyUsedByAnotherUserException : Exception
{
    public EmailAddressAlreadyUsedByAnotherUserException() : base(
        "The provided email address is already used by another user.")
    {
    }
}
