using Bootstrap.Tests.Acceptance.Utils;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;
using TechTalk.SpecFlow;

namespace Bootstrap.Tests.Acceptance.Steps;

[Binding]
public class ErrorSteps
{
    private readonly ErrorDriver errorDriver;

    public ErrorSteps(ErrorDriver errorDriver)
    {
        this.errorDriver = errorDriver;
    }

    /// <summary>
    /// Usage :
    /// <br/> - An error is thrown
    /// <br/> - An error is thrown with message "my message"
    /// <br/> - An error is thrown with type MyErrorType
    /// <br/> - An error is thrown with type MyErrorType and message "my message"
    /// <br/> - A not found error is thrown
    /// <br/> - A not found error is thrown with type MyErrorType
    /// <br/> - A not found error is thrown with message "The id (.*) was not found"
    /// <br/> - A not found error is thrown with type MyErrorType and message "The id (.*) was not found"
    ///
    /// </summary>
    /// <param name="statusCode"></param>
    /// <param name="errorType"></param>
    /// <param name="message"></param>
    [Then(
        "(?:an|an? (?<statusCode>.*)) error occurred"
        + @"(?: with)?(?: type (?<errorType>\S*))?"
        + @"(?: and)?(?: message ""?(?<message>[^""]*)""?)?"
    )]
    public void ThenAnErrorIsThrownWithCodeAndMessage(string statusCode, string errorType, string message)
    {
        var error = errorDriver.GetLastError();

        error.Should().NotBeNull();

        if (!string.IsNullOrWhiteSpace(errorType))
        {
            error.ErrorType.Should().Be(errorType);
        }

        if (!string.IsNullOrWhiteSpace(statusCode))
        {
            error.StatusCode
                .Should()
                .Be(HumanizedHelper.ParseEnum<HttpStatusCode>(statusCode), $"it should be a {statusCode} error.");
        }

        if (!string.IsNullOrWhiteSpace(message))
        {
            error.ProblemDetails.Title.Should().NotBeNull();
            error.ProblemDetails.Title.Should().MatchRegex(message);
        }
    }

    [AfterScenario]
    public void ThrowIfNotCatched()
    {
        errorDriver.ThrowIfNotProcessedException();
    }
}