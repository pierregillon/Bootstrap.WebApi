using System.Text.Json.Nodes;
using Bootstrap.Application;
using Bootstrap.Tests.Acceptance.Utils;
using FluentAssertions;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace Bootstrap.Tests.Acceptance.Steps;

[Binding]
public class CustomerSteps : StepBase
{
    public CustomerSteps(ScenarioContext context) : base(context)
    {
    }

    [When(@"I register the following new customer")]
    public async Task WhenIRegisterTheFollowingNewCustomer(Table table)
    {
        var data = table.CreateInstance<CustomerSpecflowData>();

        await this.Client.Post("api/customers", new { firstName = data.FirstName, lastName = data.LastName });
    }

    [Then(@"the customer list is")]
    public async Task ThenTheCustomerListIs(Table table)
    {
        var actualCustomers = await Client.Get<IEnumerable<CustomerListItemSpecflowData>>("api/customers");

        var expectedCustomers = table.CreateSet<CustomerListItemSpecflowData>();

        actualCustomers
            .Should()
            .BeEquivalentTo(expectedCustomers);
    }
}

public record CustomerSpecflowData(string FirstName, string LastName);
public record CustomerListItemSpecflowData(string FullName);