using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace Bootstrap.Tests.Acceptance.Utils;

public class HttpError
{
    public HttpError(Exception exception)
    {
        InnerException = exception;
        ProblemDetails = new ProblemDetails {Title = exception.Message, Detail = exception.Message};
    }

    public string ErrorType { get; set; }
    public HttpStatusCode StatusCode { get; set; }
    public string Content { get; set; }
    public ProblemDetails ProblemDetails { get; set; }
    public Exception InnerException { get; }
}
