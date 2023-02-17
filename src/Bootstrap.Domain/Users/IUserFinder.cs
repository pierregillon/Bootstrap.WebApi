namespace Bootstrap.Domain.Users;

public interface IUserFinder
{
    Task<bool> AnyUserAlreadyRegisteredWith(EmailAddress emailAddress);
    Task<RegisteredUserDto?> FindUser(UserId userId);
}

public record RegisteredUserDto;
