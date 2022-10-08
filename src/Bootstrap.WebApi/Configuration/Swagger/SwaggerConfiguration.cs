namespace Bootstrap.WebApi.Configuration.Swagger;

public static class SwaggerConfiguration
{
    public static IServiceCollection ConfigureSwaggerServices(this IServiceCollection services, IConfiguration configuration)
    {
        if (!configuration.IsSwaggerEnabled())
        {
            return services;
        }

        services.AddSwaggerGen();
        services.ConfigureOptions<ConfigureSwaggerOptions>();
        services.ConfigureOptions<ConfigureSwaggerUIOptions>();

        return services;
    }

    public static WebApplication ConfigureSwagger(this WebApplication app)
    {
        if (!app.Configuration.IsSwaggerEnabled())
        {
            return app;
        }

        app.UseSwagger();
        app.UseSwaggerUI();

        return app;
    }
}
