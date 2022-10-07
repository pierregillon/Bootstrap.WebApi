using Microsoft.AspNetCore.Mvc.Testing;
using TechTalk.SpecFlow;

namespace Bootstrap.Tests.Acceptance;

[Binding]
public class ScenarioInitializer : IDisposable
{
    private readonly ScenarioContext _scenarioContext;
    private WebApplicationFactory<WebApi.Program>? _application;

    public ScenarioInitializer(ScenarioContext scenarioContext) => _scenarioContext = scenarioContext;

    [BeforeScenario]
    public void InitializeAsync()
    {
        _application = new AcceptanceTestApplication();
        _scenarioContext.Set(_application.Server.CreateClient());
    }

    protected virtual void Dispose(bool disposing)
    {
        if (disposing)
        {
            _application?.Dispose();
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
