using System.Text;
using Bootstrap.Infrastructure.Context;
using Bootstrap.WebApi.Configuration.Authentication.ApiKey;
using Bootstrap.WebApi.Configuration.Authentication.Bearer;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace Bootstrap.WebApi.Configuration.Authentication;

internal static class DependencyInjection
{
    public static IServiceCollection ConfigureAuthentication(this IServiceCollection services,
        IConfiguration configuration)
    {
        var jwtTokenOptions = new JwtTokenOptions();

        configuration.Bind(JwtTokenOptions.Section, jwtTokenOptions);

        services
            .AddAuthentication(
                options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                })
            .AddJwtBearer(
                options =>
                {
                    options.SaveToken = true;
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidAudience = jwtTokenOptions.ValidAudience,
                        ValidIssuer = jwtTokenOptions.ValidIssuer,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtTokenOptions.Secret))
                    };
                });

        services.AddScoped<JwtTokenGenerator>();
        services.AddOptions<JwtTokenOptions>().BindConfiguration(JwtTokenOptions.Section);

        services
            .AddScoped<InMemoryUserContext>()
            .AddScoped<IUserContext>(x => x.GetRequiredService<InMemoryUserContext>())
            .AddScoped<ValidateUserExistenceMiddleware>()
            .AddScoped<InitializeUserContextMiddleware>()
            ;

        services
            .AddOptions<SecurityConfiguration>()
            .BindConfiguration(SecurityConfiguration.Section)
            .ValidateDataAnnotations();

        return services;
    }

    public static IApplicationBuilder UseCustomAuthentication(this IApplicationBuilder application) =>
        application
            .UseAuthentication()
            // .UseMiddleware<CompleteHttpRequestTelemetryMiddleware>()
            .UseMiddleware<ValidateUserExistenceMiddleware>()
            .UseMiddleware<InitializeUserContextMiddleware>();
}
