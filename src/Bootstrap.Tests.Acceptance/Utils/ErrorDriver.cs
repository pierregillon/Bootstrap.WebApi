using TechTalk.SpecFlow;

namespace Bootstrap.Tests.Acceptance.Utils;

public class ErrorDriver
{
    private readonly ScenarioInfo scenarioInfo;
    private Queue<HttpError> errors = new();

    public ErrorDriver(ScenarioInfo scenarioInfo)
    {
        this.scenarioInfo = scenarioInfo;
    }

    private bool InErrorScenario => this.scenarioInfo.Tags.Contains("ErrorHandling");

    public async Task TryExecute(Func<Task> action)
    {
        if (!this.InErrorScenario)
        {
            await action();
        }
        else
        {
            try
            {
                await action();
            }
            catch (Exception ex)
            {
                this.errors.Enqueue(new HttpError(ex));
            }
        }
    }

    public HttpError GetLastError()
    {
        if (!this.errors.TryDequeue(out var error))
        {
            throw new InvalidOperationException("No error has been thrown but it should");
        }

        return error;
    }

    public void ThrowIfNotProcessedException()
    {
        if (!this.InErrorScenario)
        {
            if (this.errors.Any())
            {
                throw this.errors.Dequeue().InnerException;
            }
        }
    }
}