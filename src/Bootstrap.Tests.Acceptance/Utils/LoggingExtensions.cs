using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;

namespace Bootstrap.Tests.Acceptance.Utils;

internal static class LoggingExtensions
{
    internal static IWebHostBuilder UseLogger(this IWebHostBuilder webHostBuilder) =>
        webHostBuilder.ConfigureLogging(
            builder =>
                builder.AddSimpleConsole(option =>
                {
                    option.IncludeScopes = false;
                    option.TimestampFormat = "hh:mm:ss ";
                })
        );
}
