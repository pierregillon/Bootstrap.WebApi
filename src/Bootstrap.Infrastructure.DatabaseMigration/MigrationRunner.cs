using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Bootstrap.Infrastructure.DatabaseMigration;

public class MigrationRunner
{
    private readonly IHost _host;

    public MigrationRunner(IHost host) => _host = host;

    public void MigrateUp()
    {
        using (var scope = _host.Services.CreateScope())
        {
            var services = scope.ServiceProvider;

            services.GetRequiredService<IMigrationRunner>().MigrateUp();
        }
    }
}
