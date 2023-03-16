using Bootstrap.Domain.Users;

namespace Bootstrap.Infrastructure.Emailing.EmailDelivery;

public record EmailSender(EmailAddress EmailAddress, string Name)
{
    public static EmailSender SupportTeam =>
        new(EmailAddress.Create("support@test.com"), "Best team");
}
