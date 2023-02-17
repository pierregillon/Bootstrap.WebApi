using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace Bootstrap.WebApi.Configuration.Swagger;

public class ConfigureSwaggerUIOptions : IConfigureNamedOptions<SwaggerUIOptions>
{
    private readonly IApiVersionDescriptionProvider _provider;

    public ConfigureSwaggerUIOptions(IApiVersionDescriptionProvider provider) => _provider = provider;

    public void Configure(SwaggerUIOptions options)
    {
        foreach (var description in _provider.ApiVersionDescriptions)
        {
            var version = description.GroupName;
            options.SwaggerEndpoint($"/swagger/{version}/swagger.json", version);
        }
    }

    public void Configure(string? name, SwaggerUIOptions options) => Configure(options);
}
