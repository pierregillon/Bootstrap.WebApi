namespace Bootstrap.Domain.Users.Security;

public interface IPasswordHasher
{
    PasswordHash Hash(Password password);
    bool VerifyHash(UnverifiedPassword password, PasswordHash passwordHash);
}
