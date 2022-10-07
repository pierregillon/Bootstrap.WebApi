using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Bootstrap.Tests.Acceptance.Utils;

public class HttpError
{
    public HttpError(Exception exception)
    {
        this.InnerException = exception;
        this.ProblemDetails = new ProblemDetails
        {
            Title = exception.Message,
            Detail = exception.Message
        };
    }

    public string ErrorType { get; set; }
    public HttpStatusCode StatusCode { get; set; }
    public string Content { get; set; }
    public ProblemDetails ProblemDetails { get; set; }
    public Exception InnerException { get; }
}