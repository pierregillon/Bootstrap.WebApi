using Bootstrap.Domain;
using Bootstrap.Domain.Users.Security;
using Microsoft.Extensions.DependencyInjection;

namespace Bootstrap.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection RegisterInfrastructure(this IServiceCollection services)
    {
        services
            .AddScoped<IPasswordHasher, AspNetCoreIdentityPasswordHasher>()
            .AddScoped<IClock, SystemClock>()
            ;

        return services;
    }
}
