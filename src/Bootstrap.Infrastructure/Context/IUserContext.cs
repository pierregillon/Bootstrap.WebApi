namespace Bootstrap.Infrastructure.Context;

public interface IUserContext
{
    CurrentUser GetCurrentUser();
}
