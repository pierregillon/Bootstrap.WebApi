using Bootstrap.Tests.Acceptance.Utils;
using FluentAssertions;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace Bootstrap.Tests.Acceptance.Steps;

[Binding]
public class CustomerSteps : StepBase
{
    private readonly IDictionary<Guid, CustomerSpecflowData> _customers = new Dictionary<Guid, CustomerSpecflowData>();

    public CustomerSteps(TestClient client) : base(client)
    {
    }

    [Given(@"the following new customer registered")]
    [When(@"I register the following new customer")]
    public async Task WhenIRegisterTheFollowingNewCustomer(Table table)
    {
        var data = table.CreateInstance<CustomerSpecflowData>();

        var customerId = await Client.Post("v1/customers", new {firstName = data.FirstName, lastName = data.LastName});

        _customers.Add(customerId, data);
    }

    [When(@"I rename the customer ""([^""]*)"" to ""([^""]*)""")]
    public async Task WhenIRenameTheCustomerTo(string originalFullName, string newFullName)
    {
        var customerId = GetJustCreatedCustomerIdFromFullName(originalFullName);

        var split = newFullName.Split(' ');
        var firstName = split.First();
        var lastName = split.Last();

        await Client.Put($"v1/customers/{customerId}/rename", new {firstName, lastName});
    }

    [When(@"I rename an unknown customer to ""([^""]*)""")]
    public async Task WhenIRenameAnUnknownCustomerTo(string newFullName)
    {
        var split = newFullName.Split(' ');
        var firstName = split.First();
        var lastName = split.Last();

        await Client.Put($"v1/customers/{Guid.NewGuid()}/rename", new {firstName, lastName});
    }

    [Then(@"the customer list is")]
    public async Task ThenTheCustomerListIs(Table table)
    {
        var actualCustomers = await Client.Get<IEnumerable<CustomerListItemSpecflowData>>("v1/customers");

        var expectedCustomers = table.CreateSet<CustomerListItemSpecflowData>();

        actualCustomers
            .Should()
            .BeEquivalentTo(expectedCustomers, options => options.Excluding(x => x.Id));
    }

    [Then(@"the ""([^""]*)"" customer is now listed with full name ""([^""]*)""")]
    public async Task ThenTheCustomerNowIsListedWithFullName(string originalFullName, string newFullName)
    {
        var customerId = this.GetJustCreatedCustomerIdFromFullName(originalFullName);

        var actualCustomers = await Client.Get<IEnumerable<CustomerListItemSpecflowData>>("v1/customers");

        actualCustomers
            .Single(x => x.Id == customerId)
            .FullName
            .Should()
            .Be(newFullName);
    }

    [Then(@"the ""([^""]*)"" customer is listed")]
    public async Task ThenTheCustomerIsListed(string fullName)
    {
        var customerId = this.GetJustCreatedCustomerIdFromFullName(fullName);

        var actualCustomers = await Client.Get<IEnumerable<CustomerListItemSpecflowData>>("v1/customers");

        actualCustomers
            .Should()
            .Contain(x => x.Id == customerId);
    }

    private Guid GetJustCreatedCustomerIdFromFullName(string originalFullName)
    {
        var customer = _customers.Single(x => x.Value.FirstName + " " + x.Value.LastName == originalFullName);

        return customer.Key;
    }
}

public record CustomerSpecflowData(string FirstName, string LastName);

public record CustomerListItemSpecflowData(Guid Id, string FullName);
