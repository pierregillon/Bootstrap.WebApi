using Bootstrap.Tests.Acceptance.Configuration;
using Bootstrap.Tests.Acceptance.Utils;
using Bootstrap.WebApi.Configuration.Authentication.Bearer;
using FluentAssertions;
using FluentAssertions.Extensions;
using Microsoft.Extensions.Options;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace Bootstrap.Tests.Acceptance.Steps;

[Binding]
public class UserSteps : StepBase
{
    private JwtTokenJson? _token;

    public UserSteps(TestClient client, TestApplication application) : base(client, application)
    {
    }

    [When(@"I register with email ""(.*)""")]
    public async Task WhenIRegisterWithEmail(string emailAddress) => await Register(emailAddress, "test");

    [When(@"I register with password ""(.*)""")]
    public async Task WhenIRegisterWithPassword(string password) => await Register("test@test.fr", password);

    [Given(@"I am registered and logged in with")]
    [When(@"I register and log in with")]
    public async Task WhenIRegisterAndLogInWith(Table table)
    {
        var (data, token) = await RegisterFromTable(table);

        if (token is not null)
        {
            Client.DefineToken(data.EmailAddress, token.Token);
        }

        _token = token;
    }

    [Given(@"the token validity is (.*) days")]
    public void GivenTheTokenValidityIsDays(int dayCount)
    {
        var options = GetService<IOptions<JwtTokenOptions>>();

        options.Value.Validity = TimeSpan.FromDays(dayCount);
    }

    [Then(@"I can now use the app until the (.*)")]
    public void ThenICanNowUserTheAppUntilThe(DateTime date)
    {
        _token.Should().NotBeNull();
        _token!.Token.Should().NotBeNull();
        _token!.Expiration.Should().Be(date.AsUtc());
    }

    private async Task<(UserInfo Data, JwtTokenJson? Token)> RegisterFromTable(Table table)
    {
        var data = table.CreateInstance<UserInfo>();

        var token = await Register(data.EmailAddress, data.Password);

        return (data, token);
    }

    private async Task<JwtTokenJson> Register(string email, string password)
        => await Client.Post<JwtTokenJson>("v1/users/register", new { email, password });

    public record UserInfo(string EmailAddress, string PhoneNumber, string Password);
}
