using System.Security.Claims;
using Bootstrap.Domain.Users;

namespace Bootstrap.WebApi.Configuration.Authentication.Bearer;

public static class HttpContextExtensions
{
    public static bool TryGetAuthenticatedUserId(this HttpContext context, out UserId userId)
    {
        var claim = context.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);
        if (claim == null)
        {
            userId = null!;
            return false;
        }

        userId = new UserId(Guid.Parse(claim.Value));
        return true;
    }
}
