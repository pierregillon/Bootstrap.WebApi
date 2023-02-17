using Bootstrap.Domain.Users.Security;
using Microsoft.AspNetCore.Identity;

namespace Bootstrap.Infrastructure;

internal class AspNetCoreIdentityPasswordHasher : IPasswordHasher
{
    private readonly PasswordHasher<AspNetCoreIdentityPasswordHasher> _passwordHasher = new();

    public PasswordHash Hash(Password password)
    {
        var hash = _passwordHasher.HashPassword(this, password);
        return PasswordHash.Create(hash);
    }

    public bool VerifyHash(UnverifiedPassword password, PasswordHash passwordHash) =>
        _passwordHasher.VerifyHashedPassword(this, passwordHash, password) != PasswordVerificationResult.Failed;
}
