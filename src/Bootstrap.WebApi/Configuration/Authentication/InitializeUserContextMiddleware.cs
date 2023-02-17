using Bootstrap.Infrastructure.Context;
using Bootstrap.WebApi.Configuration.Authentication.Bearer;

namespace Bootstrap.WebApi.Configuration.Authentication;

public class InitializeUserContextMiddleware : IMiddleware
{
    private readonly InMemoryUserContext _userContext;

    public InitializeUserContextMiddleware(InMemoryUserContext userContext) => _userContext = userContext;

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        if (context.TryGetAuthenticatedUserId(out var userId))
        {
            _userContext.InitializeCurrentUser(new CurrentUser(userId));
        }

        await next(context);
    }
}
