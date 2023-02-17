using Bootstrap.Domain.Users;

namespace Bootstrap.Infrastructure.Context;

public class InMemoryUserContext : IUserContext
{
    private CurrentUser? _currentUser;

    public CurrentUser GetCurrentUser() =>
        _currentUser ?? throw new InvalidOperationException("There is no current user from context.");

    public void InitializeCurrentUser(CurrentUser user) => _currentUser = user;
}

public record CurrentUser(UserId UserId);
