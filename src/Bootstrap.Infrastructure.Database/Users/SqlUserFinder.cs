using Bootstrap.Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace Bootstrap.Infrastructure.Database.Users;

internal class SqlUserFinder : IUserFinder
{
    private readonly BootstrapDbContext _dbContext;

    public SqlUserFinder(BootstrapDbContext dbContext) => _dbContext = dbContext;

    public async Task<bool> AnyUserAlreadyRegisteredWith(EmailAddress emailAddress) =>
        await _dbContext.Users
            .Where(x => x.EmailAddress == emailAddress)
            .AnyAsync();

    public async Task<RegisteredUserDto?> FindUser(UserId userId)
    {
        var entity = await _dbContext.Users.FindAsync((Guid)userId);
        return entity is null ? null : new RegisteredUserDto();
    }
}
