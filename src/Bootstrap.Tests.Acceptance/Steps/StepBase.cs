using Bootstrap.Tests.Acceptance.Configuration;
using Bootstrap.Tests.Acceptance.Utils;

namespace Bootstrap.Tests.Acceptance.Steps;

public abstract class StepBase
{
    protected readonly TestClient Client;
    private readonly TestApplication _application;

    protected StepBase(TestClient client, TestApplication application)
    {
        Client = client;
        _application = application;
    }

    protected T GetService<T>() => (T)_application.Services.GetService(typeof(T))!;

}
