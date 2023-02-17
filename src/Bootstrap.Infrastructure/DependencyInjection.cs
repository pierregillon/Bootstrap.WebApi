using Bootstrap.Domain;
using Bootstrap.Domain.Customers;
using Bootstrap.Domain.Users;
using Bootstrap.Domain.Users.Security;
using Bootstrap.Infrastructure.Database;
using Bootstrap.Infrastructure.Database.Customers;
using Bootstrap.Infrastructure.Database.Users;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Bootstrap.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection RegisterInfrastructure(this IServiceCollection services)
    {
        services
            .AddScoped<ICustomerRepository, SqlCustomerRepository>()
            .AddScoped<IUserRepository, SqlUserRepository>()
            .AddScoped<IUserFinder, SqlUserFinder>()
            .AddScoped<IPasswordHasher, AspNetCoreIdentityPasswordHasher>()
            .AddScoped<IClock, SystemClock>()
            .AddDbConfigurations()
            .AddTransient(typeof(IPipelineBehavior<,>), typeof(UnitOfWorkBehavior<,>))
            .AddDbContext<BootstrapDbContext>((sp, options) =>
            {
                var configuration = sp.GetRequiredService<IOptions<DatabaseConfiguration>>();

                options
                    .UseNpgsql(configuration);
            })
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
