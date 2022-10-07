using Bootstrap.Tests.Acceptance.Utils;

namespace Bootstrap.Tests.Acceptance.Steps;

public abstract class StepBase
{
    protected readonly TestClient Client;

    protected StepBase(TestClient client) => Client = client;
}
