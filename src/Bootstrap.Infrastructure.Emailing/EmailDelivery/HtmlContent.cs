namespace Bootstrap.Infrastructure.Emailing.EmailDelivery;

public record HtmlContent(string Value)
{
    public static implicit operator string(HtmlContent content) => content.Value;
}
