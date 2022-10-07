using System.Net;
using Bootstrap.Tests.Acceptance.Utils;
using FluentAssertions;
using TechTalk.SpecFlow;

namespace Bootstrap.Tests.Acceptance.Steps;

[Binding]
public class ErrorSteps
{
    private readonly ErrorDriver _errorDriver;

    public ErrorSteps(ErrorDriver errorDriver) => _errorDriver = errorDriver;

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
        var error = _errorDriver.GetLastError();

        if (!string.IsNullOrWhiteSpace(errorType))
        {
            error.ProblemDetails.Type.Should().Be(errorType);
        }

        if (!string.IsNullOrWhiteSpace(statusCode))
        {
            error.HttpStatusCode
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
    public void ThrowIfNotProcessedException() => _errorDriver.ThrowIfNotProcessedException();
}
