namespace Bootstrap.WebApi.Configuration.Authentication.Bearer;

public class JwtTokenOptions
{
    public const string Section = "JWT";

    public string ValidAudience { get; set; } = string.Empty;
    public string ValidIssuer { get; set; } = string.Empty;
    public string Secret { get; set; } = string.Empty;
    public TimeSpan Validity { get; set; } = TimeSpan.FromDays(1);
}
