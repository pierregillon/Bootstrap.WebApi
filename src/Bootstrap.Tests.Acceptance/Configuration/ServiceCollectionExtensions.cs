using Bootstrap.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;

namespace Bootstrap.Tests.Acceptance.Configuration;

public static class ServiceCollectionExtensions
{
    public static void AddEntityFrameworkInMemory(this IServiceCollection services)
    {
        var databaseName = Guid.NewGuid()
            .ToString();

        services.RemoveWhere(x => x.ServiceType == typeof(DbContextOptions<BootstrapDbContext>));

        services.AddDbContext<BootstrapDbContext>(options =>
        {
            options.UseInMemoryDatabase(databaseName);
            options.ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning));
        });
    }

    private static void RemoveWhere(this IServiceCollection services, Func<ServiceDescriptor, bool> filter)
    {
        var toRemove = services.Where(filter)
            .ToArray();

        foreach (var serviceDescriptor in toRemove)
        {
            services.Remove(serviceDescriptor);
        }
    }
}
