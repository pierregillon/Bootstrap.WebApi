using FluentMigrator.Runner;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NSubstitute;
using TechTalk.SpecFlow.Infrastructure;

namespace Bootstrap.Tests.Acceptance.Configuration;

public class TestApplication : TestApplicationBase
{
    public TestApplication(ISpecFlowOutputHelper specFlowOutputHelper) : base(specFlowOutputHelper) { }

    protected override void OverrideAcceptanceServices(IServiceCollection services)
    {
        services.AddEntityFrameworkInMemory();
    }

    protected override void OverrideAcceptanceTestServices(IServiceCollection services)
    {
        services
            .RemoveAll<IMigrationRunner>()
            .AddSingleton(_ => Substitute.For<IMigrationRunner>());
    }

    protected override void OverrideIntegrationServices(IServiceCollection services)
    {
        
    }

    protected override void OverrideIntegrationTestServices(IServiceCollection services)
    {
    }
}
