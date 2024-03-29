﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace Bootstrap.Infrastructure.Database;

public static class HealthChecksExtension
{
    public static IHealthChecksBuilder AddDatabaseCheck(this IHealthChecksBuilder builder) =>
        builder.AddDbContextCheck<BootstrapDbContext>(
            $"{nameof(BootstrapDbContext)} readiness check",
            HealthStatus.Unhealthy,
            new[] { "persistence", "database", "readiness" }
        );
}
