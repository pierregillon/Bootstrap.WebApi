using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace Bootstrap.Tests.Acceptance.Utils;

public class HttpError
{
    public HttpError(HttpException exception)
    {
        InnerException = exception;
        ProblemDetails = exception.ProblemDetails;
    }

    public ProblemDetails ProblemDetails { get; }
    public Exception InnerException { get; }
    public HttpStatusCode HttpStatusCode =>
        (HttpStatusCode)(this.ProblemDetails.Status ?? throw new InvalidOperationException("No status"));
}
