using Bootstrap.Infrastructure.Database;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Bootstrap.WebApi.Configuration;

public static class HealthCheckConfiguration
{
    public static IApplicationBuilder UseHealthChecksRoutes(this IApplicationBuilder builder) =>
        builder
            .UseHealthChecks(
                "/liveness",
                new HealthCheckOptions
                {
                    Predicate = r => r.Tags.Contains("liveness"),
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,
                    ResultStatusCodes =
                    {
                        [HealthStatus.Healthy] = StatusCodes.Status200OK,
                        [HealthStatus.Degraded] = StatusCodes.Status200OK,
                        [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable
                    }
                }
            )
            .UseHealthChecks(
                "/hc",
                new HealthCheckOptions
                {
                    Predicate = r => r.Tags.Contains("readiness"),
                    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse,
                    ResultStatusCodes =
                    {
                        [HealthStatus.Healthy] = StatusCodes.Status200OK,
                        [HealthStatus.Degraded] = StatusCodes.Status200OK,
                        [HealthStatus.Unhealthy] = StatusCodes.Status503ServiceUnavailable
                    }
                }
            );

    public static IServiceCollection AddServiceHealthChecks(this IServiceCollection services)
    {
        services
            .AddHealthChecks()
            .AddLivenessChecks()
            .AddReadinessChecks();

        return services;
    }

    private static IHealthChecksBuilder AddLivenessChecks(this IHealthChecksBuilder builder) =>
        builder.AddCheck(
            "Api liveness check",
            () => HealthCheckResult.Healthy("the service is staying alive! Disco!"),
            new[] { "api", "liveness" }
        );

    private static IHealthChecksBuilder AddReadinessChecks(this IHealthChecksBuilder builder) =>
        builder.AddDatabaseCheck();
}
