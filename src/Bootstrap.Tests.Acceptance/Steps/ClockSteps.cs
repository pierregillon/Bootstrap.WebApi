using Bootstrap.Domain;
using Bootstrap.Tests.Acceptance.Configuration;
using Bootstrap.Tests.Acceptance.Utils;
using FluentAssertions.Extensions;
using NSubstitute;
using TechTalk.SpecFlow;

namespace Bootstrap.Tests.Acceptance.Steps;

[Binding]
public class ClockSteps : StepBase
{
    public ClockSteps(TestClient client, TestApplication application) : base(client, application)
    {
    }

    [Given(@"the current date is (.*)")]
    public void GivenTheCurrentDateIs(DateTime now) => GetService<IClock>().Now().Returns(now.AsUtc());
}
