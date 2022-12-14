using Bootstrap.Application;
using Bootstrap.BuildingBlocks;
using Bootstrap.Infrastructure;
using Bootstrap.Infrastructure.DatabaseMigration;
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
    .RegisterDatabaseMigration()
    ;

builder.Services
    .AddServiceHealthChecks()
    .AddApplicationInsightsTelemetry();

var app = builder.Build();

app
    .ConfigureSwagger()
    .UseHealthChecksRoutes()
    .UseExceptionHandler("/internal/error");

app.UseAuthorization();
app.MapControllers();

app.MigrationRunner().MigrateUp();
app.Run();

namespace Bootstrap.WebApi
{
    public partial class Program { }
}
