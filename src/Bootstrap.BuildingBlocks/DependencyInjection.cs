using Bootstrap.BuildingBlocks.Commands;
using Bootstrap.BuildingBlocks.Queries;
using Microsoft.Extensions.DependencyInjection;

namespace Bootstrap.BuildingBlocks;

public static class DependencyInjection
{
    public static IServiceCollection RegisterBuildingBlocks(this IServiceCollection services)
    {
        services.AddScoped<ICommandDispatcher, MediatorDispatcher>();
        services.AddScoped<IQueryDispatcher, MediatorDispatcher>();

        return services;
    }
}
