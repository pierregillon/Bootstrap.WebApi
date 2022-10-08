using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace Bootstrap.Tests.Acceptance.Utils;

public class HttpException : Exception
{
    public ProblemDetails ProblemDetails { get; }

    public HttpException(string path, ProblemDetails problemDetails) : base($"{(HttpStatusCode)problemDetails.Status!} on {path} : {problemDetails.Title ?? "No details"}")
    {
        ProblemDetails = problemDetails;
    }

    public static Exception From(string path, HttpStatusCode statusCode) 
        => new HttpException(path, new ProblemDetails {Status = (int)statusCode });

    public static Exception From(string path, ProblemDetails problemDetails) 
        => new HttpException(path, problemDetails);
}
