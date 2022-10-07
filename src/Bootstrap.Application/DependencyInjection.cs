using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Bootstrap.Application;

public static class DependencyInjection
{
    public static IServiceCollection RegisterApplication(this IServiceCollection services)
    {
        services.AddMediatR(typeof(DependencyInjection).Assembly);

        return services;
    }
}
