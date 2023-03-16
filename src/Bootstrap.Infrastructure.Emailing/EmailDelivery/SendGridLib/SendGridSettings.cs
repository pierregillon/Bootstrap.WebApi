using System.ComponentModel.DataAnnotations;

namespace Bootstrap.Infrastructure.Emailing.EmailDelivery.SendGridLib;

public class SendGridSettings
{
    public const string ConfigurationName = "SendGrid";

    [Required] public string? ApiKey { get; set; }

    [Required] public string? Host { get; set; }
}
