using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;

namespace Bootstrap.WebApi.Configuration.Authentication.ApiKey;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class ApiKeyAuthorizeAttribute : Attribute, IAsyncActionFilter
{
    private const string ApiKeyHeaderName = "X-Api-Key";

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (!context.HttpContext.Request.Headers.TryGetValue(ApiKeyHeaderName, out var extractedApiKey))
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        var securityConfiguration =
            context.HttpContext.RequestServices.GetRequiredService<IOptions<SecurityConfiguration>>();
        var definedApiKey = securityConfiguration.Value.ApiKey;
        var providedApiKey = extractedApiKey.FirstOrDefault();

        if (!definedApiKey.Equals(providedApiKey))
        {
            context.Result = new UnauthorizedResult();
            return;
        }

        await next();
    }
}
