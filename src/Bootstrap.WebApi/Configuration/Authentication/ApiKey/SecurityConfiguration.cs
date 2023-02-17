using System.ComponentModel.DataAnnotations;

namespace Bootstrap.WebApi.Configuration.Authentication.ApiKey;

public class SecurityConfiguration
{
    public const string Section = "Security";
    [Required] public string ApiKey { get; set; } = string.Empty;
}
