using Bootstrap.Infrastructure.Emailing.EmailDelivery;

namespace Bootstrap.Tests.Acceptance.Fakes;

public class FakeEmailSender : IEmailSender
{
    private readonly List<Email> _sentEmails = new();
    private bool _isDown;

    public IReadOnlyCollection<Email> SentEmails => _sentEmails;

    public Task Send(Email email)
    {
        if (_isDown)
        {
            throw new InvalidOperationException("Fake email sender is down");
        }

        _sentEmails.Add(email);
        return Task.CompletedTask;
    }

    public void SimulateIsDown() => _isDown = true;

    public void Clear() => _sentEmails.Clear();
}
