using SendGrid;

namespace Bootstrap.Infrastructure.Emailing.EmailDelivery;

internal class FailedToDeliverEmailException : Exception
{
    public FailedToDeliverEmailException(Email email, Response response, Dictionary<string, object>? body)
        : base(string.Concat(BuildMessage(email, response, body)))
    {
        Email = email;
        Response = response;
        Body = body;
    }

    public Email Email { get; }
    public Response Response { get; }
    public Dictionary<string, object>? Body { get; }

    private static IEnumerable<string> BuildMessage(Email email, Response response, Dictionary<string, object>? body)
    {
        yield return $"The email to {email.To} failed to be sent with status {response.StatusCode}.";

        if (body is not null)
        {
            var reason = string.Join(", ", body.Select(x => $"{x.Key}:{x.Value}"));
            yield return $" Reason = {reason}";
        }
    }
}
