using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace Bootstrap.Tests.Acceptance.Utils;

public class HttpException : Exception
{
    public ProblemDetails ProblemDetails { get; }

    public HttpException(string path, ProblemDetails problemDetails) : base($"{(HttpStatusCode)problemDetails.Status!} on {path} : {problemDetails.Title}")
    {
        ProblemDetails = problemDetails;
    }
}
