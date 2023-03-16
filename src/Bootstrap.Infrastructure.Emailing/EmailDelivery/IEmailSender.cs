namespace Bootstrap.Infrastructure.Emailing.EmailDelivery;

public interface IEmailSender
{
    Task Send(Email email);
}
