using System.Diagnostics;
using Bootstrap.Domain.Users;
using Bootstrap.Domain.Users.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using OpenTelemetry.Trace;

namespace Bootstrap.WebApi.Controllers;

[ApiController]
[AllowAnonymous]
public class ErrorController : ProblemDetailsController
{
    public ErrorController(ProblemDetailsFactory problemDetailsFactory, IHostEnvironment hostEnvironment) : base(
        problemDetailsFactory, hostEnvironment)
    {
    }

    [Route("internal/error")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public IActionResult Error()
    {
        var context = HttpContext.Features.Get<IExceptionHandlerFeature>();

        AdjustActivityMetadata(context);

        return Handle(context?.Error);
    }

    private static void AdjustActivityMetadata(IExceptionHandlerFeature? context)
    {
        if (Activity.Current is null)
        {
            return;
        }

        var exception = context?.Error;

        // This allow to visualize correctly route in error in ApplicationInsight.
        // Else it is under route Internal/Error which is generic.
        Activity.Current.DisplayName = context?.Path ?? Activity.Current.DisplayName;
        Activity.Current.SetTag("http.route", context?.Path);
        Activity.Current.SetStatus(ActivityStatusCode.Error, exception?.Message);
        Activity.Current.RecordException(exception);
    }

    private IActionResult Handle(Exception? exception)
    {
        if (exception is null)
        {
            return HandledUnspecifiedError();
        }

        return exception switch
        {
            // LoginFailedException => BuildProblemDetails(exception, StatusCodes.Status401Unauthorized, "LoginFailed"),
            BadEmailAddressFormatException => BuildProblemDetails(exception, StatusCodes.Status400BadRequest,
                "BadEmailAddressFormat"),
            EmailAddressAlreadyUsedByAnotherUserException => BuildProblemDetails(exception,
                StatusCodes.Status409Conflict, "EmailAddressAlreadyUsedByAnotherUser"),
            TooWeakPasswordException => BuildProblemDetails(exception, StatusCodes.Status400BadRequest,
                "TooWeakPassword"),
            _ => BuildDefaultProblemDetails(exception)
        };
    }

    private IActionResult BuildDefaultProblemDetails(Exception exception)
        => BuildProblemDetails(
            exception,
            StatusCodes.Status500InternalServerError,
            "UnexpectedException"
        );

    protected IActionResult HandledUnspecifiedError()
        => BuildProblemDetails(
            "no detail available.",
            StatusCodes.Status500InternalServerError,
            "MissingException"
        );
}

public abstract class ProblemDetailsController : ControllerBase
{
    private readonly IHostEnvironment _hostEnvironment;
    private readonly ProblemDetailsFactory _problemDetailsFactory;

    protected ProblemDetailsController(ProblemDetailsFactory problemDetailsFactory, IHostEnvironment hostEnvironment)
    {
        _problemDetailsFactory = problemDetailsFactory;
        _hostEnvironment = hostEnvironment;
    }

    protected IActionResult BuildProblemDetails(Exception? exception, int statusCode, string type,
        IDictionary<string, object>? extensions = null)
    {
        var problem = CreateProblemDetails(
            statusCode,
            exception?.Message,
            type
        );

        if (_hostEnvironment.IsDevelopment() || _hostEnvironment.EnvironmentName == "test")
        {
            problem.Extensions.Add("exception", RenderException(exception));
        }

        if (extensions is not null)
        {
            foreach (var extension in extensions)
            {
                problem.Extensions.Add(extension.Key, extension.Value);
            }
        }

        return MakeResult(problem);
    }

    protected IActionResult BuildProblemDetails(string errorMessage, int statusCode, string errorType)
    {
        var problem = CreateProblemDetails(
            statusCode,
            errorMessage,
            errorType
        );

        return MakeResult(problem);
    }

    private ProblemDetails CreateProblemDetails(int httpStatusCode, string? title, string type)
    {
        var problem = _problemDetailsFactory.CreateProblemDetails(
            HttpContext,
            httpStatusCode,
            title,
            type
        );

        return problem;
    }

    private ObjectResult MakeResult(ProblemDetails problem) => StatusCode(problem.Status!.Value, problem);

    private static object RenderException(Exception? e)
    {
        if (e is null)
        {
            return new { Message = "no detail available." };
        }

        return new
        {
            e.Message,
            e.Data,
            InnerException = e.InnerException != null ? RenderException(e.InnerException) : null,
            e.StackTrace,
            HResult = e.HResult.ToString("X"),
            e.Source,
            TargetSite = e.TargetSite?.ToString()
        };
    }
}
