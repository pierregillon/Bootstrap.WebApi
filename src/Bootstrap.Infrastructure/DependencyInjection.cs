using Bootstrap.Domain.Customers;
using Bootstrap.Infrastructure.Customers;
using Bootstrap.Infrastructure.Database;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Bootstrap.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection RegisterInfrastructure(this IServiceCollection services)
    {
        services
            .AddScoped<ICustomerRepository, SqlCustomerRepository>()
            .AddDbConfigurations()
            .AddTransient(typeof(IPipelineBehavior<,>), typeof(UnitOfWorkBehavior<,>))
            .AddDbContext<BootstrapDbContext>()
            ;

        return services;
    }

    private static IServiceCollection AddDbConfigurations(this IServiceCollection services)
    {
        services
            .AddOptions<DatabaseConfiguration>()
            .BindConfiguration(DatabaseConfiguration.Section)
            .ValidateDataAnnotations();

        return services;
    }
}
