using Microsoft.Extensions.Hosting;

namespace Bootstrap.Infrastructure.DatabaseMigration;

public static class HostExtensions
{
    public static MigrationRunner MigrationRunner(this IHost host) => new(host);
}
