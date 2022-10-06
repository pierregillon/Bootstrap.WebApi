using Bootstrap.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;

namespace Bootstrap.Tests.Acceptance.Utils;

public static class ServiceCollectionExtensions
{
    public static void AddEntityFrameworkInMemory(this IServiceCollection services)
    {
        var databaseName = Guid.NewGuid()
            .ToString();

        var serviceProvider = new ServiceCollection()
            .AddEntityFrameworkInMemoryDatabase()
            .BuildServiceProvider();

        services.RemoveWhere(x => x.ServiceType == typeof(DbContextOptions<BootstrapDbContext>));

        services.AddDbContext<BootstrapDbContext>(options =>
        {
            options.UseInMemoryDatabase(databaseName);
            options.ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning));
            options.UseInternalServiceProvider(serviceProvider);
        });
    }

    public static void RemoveWhere(this IServiceCollection services, Func<ServiceDescriptor, bool> filter)
    {
        var toRemove = services.Where(filter).ToArray();

        foreach (var serviceDescriptor in toRemove)
        {
            services.Remove(serviceDescriptor);
        }
    }
}