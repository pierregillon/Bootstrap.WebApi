using Bootstrap.Tests.Acceptance.Utils;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;

namespace Bootstrap.Tests.Acceptance;

public class AcceptanceTestApplication : WebApplicationFactory<WebApi.Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        base.ConfigureWebHost(builder);

        builder
            .UseLogger()
            .UseContentRoot(AppDomain.CurrentDomain.BaseDirectory)
            .UseEnvironment("test")
            .ConfigureServices(ConfigureServices)
            .ConfigureTestServices(ConfigureTestServices)
            ;
    }

    private static void ConfigureServices(IServiceCollection services) => services.AddEntityFrameworkInMemory();

    private void ConfigureTestServices(IServiceCollection services)
    {
    }
}
