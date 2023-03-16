namespace Bootstrap.Infrastructure.Emailing.EmailDelivery;

public record EmailSubject(string Value)
{
    public static implicit operator string(EmailSubject subject) => subject.Value;
}
