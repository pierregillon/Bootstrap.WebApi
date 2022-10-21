using Bootstrap.Infrastructure.Database;
using Bootstrap.Infrastructure.DatabaseMigration.Migrations.Utils;
using FluentMigrator.Runner;
using FluentMigrator.Runner.Initialization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Bootstrap.Infrastructure.DatabaseMigration;

public static class DependencyInjection
{
    public static IServiceCollection RegisterDatabaseMigration(this IServiceCollection services)
    {
        services
            .ConfigureFluentMigrator()
            ;

        return services;
    }

    public static IServiceCollection ConfigureFluentMigrator(this IServiceCollection services) =>
        services
            .AddFluentMigratorCore()
            .Configure<RunnerOptions>(opt => {
                opt.TransactionPerSession = false;
            })
            .ConfigureRunner(
                builder => builder
                    .AddPostgres()
                    .WithGlobalConnectionString(x => x.GetRequiredService<IOptions<DatabaseConfiguration>>().Value.ConnectionString)
                    .ScanIn(typeof(MigrationAttribute).Assembly)
                    .For.All()
            )
            .AddLogging(lb => lb.AddFluentMigratorConsole());
}
