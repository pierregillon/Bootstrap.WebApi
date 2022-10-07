using TechTalk.SpecFlow;

namespace Bootstrap.Tests.Acceptance.Utils;

public class ErrorDriver
{
    private readonly ScenarioInfo _scenarioInfo;
    private readonly Queue<HttpError> _errors = new();

    public ErrorDriver(ScenarioInfo scenarioInfo) => _scenarioInfo = scenarioInfo;

    private bool InErrorScenario => _scenarioInfo.Tags.Contains("ErrorHandling");

    public async Task TryExecute(Func<Task> action)
    {
        if (!InErrorScenario)
        {
            await action();
        }
        else
        {
            try
            {
                await action();
            }
            catch (HttpException ex)
            {
                _errors.Enqueue(new HttpError(ex));
            }
        }
    }

    public HttpError GetLastError()
    {
        if (!_errors.TryDequeue(out var error))
        {
            throw new SpecFlowException("No error has been thrown but it should");
        }

        return error;
    }

    public void ThrowIfNotProcessedException()
    {
        if (_errors.Any())
        {
            throw new SpecFlowException("An error occurred during scenario but has not be processed.", _errors.Dequeue().InnerException);
        }
    }
}
