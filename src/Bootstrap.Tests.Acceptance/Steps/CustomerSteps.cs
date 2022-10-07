using Bootstrap.Tests.Acceptance.Utils;
using FluentAssertions;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace Bootstrap.Tests.Acceptance.Steps;

[Binding]
public class CustomerSteps : StepBase
{
    public CustomerSteps(TestClient client) : base(client)
    {
    }

    [Given(@"the following new customer registered")]
    [When(@"I register the following new customer")]
    public async Task WhenIRegisterTheFollowingNewCustomer(Table table)
    {
        var data = table.CreateInstance<CustomerSpecflowData>();

        await Client.Post("api/customers", new {firstName = data.FirstName, lastName = data.LastName});
    }

    [When(@"I rename the customer ""([^""]*)"" to ""([^""]*)""")]
    public async Task WhenIRenameTheCustomerTo(string originalFullName, string newFullName)
    {
        var customers = await Client.Get<IEnumerable<CustomerListItemSpecflowData>>("api/customers");

        var customer = customers.Single(x => x.FullName == originalFullName);

        var firstName = newFullName.Split(' ')
            .First();
        var lastName = newFullName.Split(' ')
            .Last();

        await Client.Put($"api/customers/{customer.Id}/rename", new {firstName, lastName});
    }

    [When(@"I rename an unknown customer to ""([^""]*)""")]
    public async Task WhenIRenameAnUnknownCustomerTo(string newFullName)
    {
        var firstName = newFullName.Split(' ')
            .First();
        var lastName = newFullName.Split(' ')
            .Last();

        await Client.Put($"api/customers/{Guid.NewGuid()}/rename", new {firstName, lastName});
    }

    [Then(@"the customer list is")]
    public async Task ThenTheCustomerListIs(Table table)
    {
        var actualCustomers = await Client.Get<IEnumerable<CustomerListItemSpecflowData>>("api/customers");

        var expectedCustomers = table.CreateSet<CustomerListItemSpecflowData>();

        actualCustomers
            .Should()
            .BeEquivalentTo(expectedCustomers, options => options.Excluding(x => x.Id));
    }
}

public record CustomerSpecflowData(string FirstName, string LastName);

public record CustomerListItemSpecflowData(Guid Id, string FullName);
