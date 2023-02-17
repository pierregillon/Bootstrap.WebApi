using Bootstrap.BuildingBlocks.Commands;
using Bootstrap.BuildingBlocks.DomainEvents;
using Bootstrap.BuildingBlocks.Queries;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Bootstrap.BuildingBlocks;

public static class DependencyInjection
{
    public static IServiceCollection RegisterBuildingBlocks(this IServiceCollection services)
    {
        services.AddScoped<ICommandDispatcher, MediatorDispatcher>();
        services.AddScoped<IQueryDispatcher, MediatorDispatcher>();
        services.AddScoped<IDomainEventPublisher, MediatorDomainEventPublisher>();

        services
            .AddTransient(typeof(IPipelineBehavior<,>), typeof(CommandActivityDecorator<,>))
            .AddTransient(typeof(IPipelineBehavior<,>), typeof(QueryActivityDecorator<,>))
            .Decorate<IDomainEventPublisher, DomainEventPublisherActivityDecorator>()
            ;
        
        return services;
    }
}
