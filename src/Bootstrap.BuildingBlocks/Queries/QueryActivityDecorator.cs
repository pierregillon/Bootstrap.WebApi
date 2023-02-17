using System.Diagnostics;
using System.Text.Json;
using MediatR;

namespace Bootstrap.BuildingBlocks.Queries;

internal class QueryActivityDecorator<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : class, IQuery<TResponse>
{
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken
    )
    {
        using Activity? activity = Activity.Current?.Source.StartActivity(request.GetType().Name);
        
        activity?.SetTag("json", JsonSerializer.Serialize(request));

        try
        {
            return await next();
        }
        catch (Exception e)
        {
            activity?.SetStatus(ActivityStatusCode.Error, e.Message);
            throw;
        }
    }
}
