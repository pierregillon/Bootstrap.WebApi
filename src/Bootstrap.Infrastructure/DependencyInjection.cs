using Bootstrap.BuildingBlocks;
using Bootstrap.Domain.Customers;
using Bootstrap.Infrastructure.Customers;
using Bootstrap.Infrastructure.EF;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;

namespace Bootstrap.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection RegisterInfrastructure(this IServiceCollection services)
    {
        services
            .AddScoped<ICustomerRepository, SqlCustomerRepository>()
            .AddDbConfigurations()
            .AddEntityFrameworkInMemory()
            .AddScoped<IUnitOfWork, UnitOfWork>()
            //.AddDbContext<BootstrapDbContext>()
            ;

        return services;
    }

    private static IServiceCollection AddDbConfigurations(this IServiceCollection services)
    {
        services
            .AddOptions<DatabaseConfiguration>()
            .BindConfiguration(DatabaseConfiguration.SectionName)
            .ValidateDataAnnotations();

        return services;
    }

    public static IServiceCollection AddEntityFrameworkInMemory(this IServiceCollection services)
    {
        var databaseName = Guid.NewGuid()
            .ToString();

        var serviceProvider = new ServiceCollection()
            .AddEntityFrameworkInMemoryDatabase()
            .BuildServiceProvider();

        services.AddDbContext<BootstrapDbContext>(options =>
        {
            options.UseInMemoryDatabase(databaseName);
            options.ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning));
            options.UseInternalServiceProvider(serviceProvider);
        });

        return services;
    }
}
