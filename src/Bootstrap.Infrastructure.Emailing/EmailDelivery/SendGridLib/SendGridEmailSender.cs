using SendGrid;
using SendGrid.Helpers.Mail;

namespace Bootstrap.Infrastructure.Emailing.EmailDelivery.SendGridLib;

internal class SendGridEmailSender : IEmailSender
{
    private readonly ISendGridClient _sendGridClient;

    public SendGridEmailSender(ISendGridClient sendGridClient) => _sendGridClient = sendGridClient;

    public async Task Send(Email email)
    {
        var message = MailHelper.CreateSingleEmail(
            new EmailAddress(email.From.EmailAddress, email.From.Name),
            new EmailAddress(email.To),
            email.Subject,
            HtmlUtilities.ConvertToPlainText(email.Content),
            email.Content
        );

        message.SetClickTracking(false, false);

        var response = await _sendGridClient.SendEmailAsync(message);

        if ((int)response.StatusCode < 200 || (int)response.StatusCode >= 300)
        {
            throw new FailedToDeliverEmailException(
                email,
                response,
                await response.DeserializeResponseBodyAsync(response.Body)
            );
        }
    }
}
