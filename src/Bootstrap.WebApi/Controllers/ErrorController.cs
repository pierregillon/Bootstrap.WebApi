using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace Bootstrap.WebApi.Controllers;

[ApiController]
[AllowAnonymous]
public class ErrorController : ControllerBase
{
    private readonly ProblemDetailsFactory _problemDetailsFactory;
    private readonly IHostEnvironment _hostEnvironment;

    public ErrorController(ProblemDetailsFactory problemDetailsFactory, IHostEnvironment hostEnvironment)
    {
        _problemDetailsFactory = problemDetailsFactory;
        _hostEnvironment = hostEnvironment;
    }

    [Route("internal/error")]
    [ApiExplorerSettings(IgnoreApi = true)]
    public IActionResult Error()
    {
        var context = HttpContext.Features.Get<IExceptionHandlerFeature>();

        if (context?.Error is not null)
        {
            return HandleException(context.Error);
        }
        else
        {
            return HandledUnspecifiedError();
        }
    }

    private IActionResult HandleException(Exception exception)
    {
        var problem = CreateProblemDetails(
            StatusCodes.Status500InternalServerError,
            exception?.Message,
            "UnexpectedException"
        );

        if (_hostEnvironment.IsDevelopment() || _hostEnvironment.EnvironmentName == "test")
        {
            problem.Extensions.Add("exception", RenderException(exception));
        }

        return MakeResult(problem);
    }

    private IActionResult HandledUnspecifiedError()
    {
        var problem = CreateProblemDetails(
            StatusCodes.Status500InternalServerError,
            "no detail available.",
            "MissingException"
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
            return new {Message = "no detail available."};
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
