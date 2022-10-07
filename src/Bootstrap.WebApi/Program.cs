using Bootstrap.Application;
using Bootstrap.BuildingBlocks;
using Bootstrap.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services
    .RegisterBuildingBlocks()
    .RegisterApplication()
    .RegisterInfrastructure();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseExceptionHandler("/internal/error");
app.UseAuthorization();
app.MapControllers();

app.Run();

namespace Bootstrap.WebApi
{
    public partial class Program
    {
    }
}
