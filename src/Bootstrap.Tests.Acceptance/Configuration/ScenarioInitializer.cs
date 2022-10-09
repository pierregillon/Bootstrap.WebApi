using TechTalk.SpecFlow;
using TechTalk.SpecFlow.UnitTestProvider;

namespace Bootstrap.Tests.Acceptance.Configuration;

[Binding]
public sealed class ScenarioInitializer : IDisposable
{
    private readonly ScenarioContext _scenarioContext;
    private readonly ScenarioInfo _scenarioInfo;
    private readonly TestApplication _application;

    public ScenarioInitializer(ScenarioContext scenarioContext, ScenarioInfo scenarioInfo, TestApplication application)
    {
        _scenarioContext = scenarioContext;
        _scenarioInfo = scenarioInfo;
        _application = application;
    }

    [BeforeScenario]
    public void InitializeAsync()
    {
        if (_application.Server.IsRunningInIntegration() && CurrentScenarioIsAcceptanceTest())
        {
            IgnoreScenario();
        }
        else
        {
            var httpClient = _application.Server.CreateClient();
            _scenarioContext.Set(httpClient);
        }
    }

    private bool CurrentScenarioIsAcceptanceTest() => !_scenarioInfo.Tags.Contains("Integration");

    private void IgnoreScenario()
    {
        var runtimeProvider = (IUnitTestRuntimeProvider)_scenarioContext.GetBindingInstance(typeof(IUnitTestRuntimeProvider));

        runtimeProvider.TestIgnore(
            "Cannot execute the current scenario: it has not been configured as integration test."
        );
    }

    public void Dispose() => _application.Dispose();
}
