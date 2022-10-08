using Bootstrap.Application;
using Bootstrap.BuildingBlocks;
using Bootstrap.Infrastructure;
using Bootstrap.WebApi.Configuration;
using Bootstrap.WebApi.Configuration.Swagger;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services
    .ConfigureVersioning()
    .ConfigureSwaggerServices(builder.Configuration);

builder.Services
    .RegisterBuildingBlocks()
    .RegisterApplication()
    .RegisterInfrastructure()
    ;

builder.Services
    .AddServiceHealthChecks();

var app = builder.Build();

app
    .ConfigureSwagger()
    .UseHealthChecksRoutes()
    .UseExceptionHandler("/internal/error");

app.UseAuthorization();
app.MapControllers();

app.Run();

namespace Bootstrap.WebApi
{
    public partial class Program
    {
    }
}
