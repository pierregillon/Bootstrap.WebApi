using TechTalk.SpecFlow;

namespace Bootstrap.Tests.Acceptance.Steps;

public abstract class StepBase
{
    private readonly ScenarioContext _context;

    protected StepBase(ScenarioContext context)
    {
        _context = context;
    }

    public HttpClient Client => _context.Get<HttpClient>();
}