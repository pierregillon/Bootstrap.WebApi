using System.Net;
using Bootstrap.Domain.Users;
using Bootstrap.WebApi.Configuration.Authentication.Bearer;

namespace Bootstrap.WebApi.Configuration.Authentication;

public class ValidateUserExistenceMiddleware : IMiddleware
{
    private readonly IUserFinder _userFinder;

    public ValidateUserExistenceMiddleware(IUserFinder userFinder) => _userFinder = userFinder;

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        if (context.TryGetAuthenticatedUserId(out var userId))
        {
            if (await _userFinder.FindUser(userId) is null)
            {
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                return;
            }
        }

        await next(context);
    }
}
