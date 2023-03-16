using Bootstrap.Domain.Users;

namespace Bootstrap.Infrastructure.Emailing.EmailDelivery;

public record Email(EmailSender From, EmailAddress To, EmailSubject Subject, HtmlContent Content);
