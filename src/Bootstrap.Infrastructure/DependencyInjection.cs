using Bootstrap.Domain;
using Bootstrap.Domain.Customers;
using Bootstrap.Infrastructure.Customers;
using Bootstrap.Infrastructure.EF;
using Microsoft.Extensions.DependencyInjection;

namespace Bootstrap.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection RegisterInfrastructure(this IServiceCollection services)
    {
        services
            .AddScoped<ICustomerRepository, SqlCustomerRepository>()
            .AddDbConfigurations()
            .AddDbContext<BootstrapDbContext>();

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
}
