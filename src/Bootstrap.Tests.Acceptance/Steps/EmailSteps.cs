using Bootstrap.Domain.Users;
using Bootstrap.Infrastructure.Emailing.EmailDelivery;
using Bootstrap.Tests.Acceptance.Configuration;
using Bootstrap.Tests.Acceptance.Fakes;
using Bootstrap.Tests.Acceptance.Utils;
using FluentAssertions;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace Bootstrap.Tests.Acceptance.Steps;

[Binding]
public class EmailSteps : StepBase
{
    public EmailSteps(TestClient client, TestApplication application) : base(client, application)
    {
    }

    private FakeEmailSender FakeEmailSender => (FakeEmailSender)GetService<IEmailSender>();

    [Given(@"the email sender is down")]
    public void GivenTheEmailSenderIsDown() => FakeEmailSender.SimulateIsDown();

    [Then(@"the following single email has been sent")]
    public void ThenTheFollowingSingleEmailHasBeenSent(Table table)
    {
        var expectedEmail = table
            .CreateInstance<SpecflowEmail>()
            .BuildEmail();

        if (FakeEmailSender.SentEmails.Count > 1)
        {
            throw new InvalidOperationException("Specflow: should be a unique email sent");
        }

        if (FakeEmailSender.SentEmails.Count == 0)
        {
            throw new InvalidOperationException("Specflow: an email should have been sent");
        }

        var actualEmail = FakeEmailSender.SentEmails.Single();

        AssertSameEmail(actualEmail, expectedEmail);
    }

    [Then(@"no email have been sent")]
    public void ThenNoEmailHaveBeenSent() => FakeEmailSender.SentEmails.Should().BeEmpty();

    private static void AssertSameEmail(Email actualEmail, Email expectedEmail)
    {
        actualEmail.To.Should().Be(expectedEmail.To);
        actualEmail.From.EmailAddress.Should().Be(expectedEmail.From.EmailAddress);
        actualEmail.From.Name.Should().BeEquivalentTo(expectedEmail.From.Name);
        actualEmail.Content.Should().BeEquivalentTo(expectedEmail.Content);

        if (!string.IsNullOrWhiteSpace(expectedEmail.Subject))
        {
            actualEmail.Subject.Should().BeEquivalentTo(expectedEmail.Subject);
        }
    }
}

public record SpecflowEmail(
    string FromEmailAddress,
    string FromName,
    string ToEmailAddress,
    string Subject,
    string HtmlContent)
{
    public Email BuildEmail()
    {
        var absolutePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "Emails", HtmlContent);

        var fileContent = File.ReadAllText(absolutePath);

        return new Email(
            new EmailSender(EmailAddress.Create(FromEmailAddress), FromName),
            EmailAddress.Create(ToEmailAddress),
            new EmailSubject(Subject),
            new HtmlContent(fileContent)
        );
    }
}
