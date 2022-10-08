namespace Bootstrap.WebApi.Configuration.Swagger;

public static class ConfigurationExtensions
{
    private const string SwaggerKey = "IS_SWAGGER_ENABLED";

    public static bool IsSwaggerEnabled(this IConfiguration configuration)
    {
        return configuration.GetSection(SwaggerKey).Value == "true";
    }
}
