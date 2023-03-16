using System.Dynamic;
using Bootstrap.Infrastructure.Emailing.EmailDelivery;
using Bootstrap.Infrastructure.Emailing.EmailDelivery.SendGridLib;
using Bootstrap.Infrastructure.Emailing.TemplateRendering;
using Bootstrap.Infrastructure.Emailing.TemplateRendering.RazorLightLib;
using Microsoft.CodeAnalysis;
using Microsoft.CSharp.RuntimeBinder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RazorLight;
using RazorLight.Extensions;
using SendGrid;

namespace Bootstrap.Infrastructure.Emailing;

public static class DependencyInjection
{
    public static IServiceCollection RegisterEmailingInfrastructure(this IServiceCollection services) =>
        services
            .AddRazorLightEngine()
            .AddEmailSender();

    private static IServiceCollection AddRazorLightEngine(this IServiceCollection services)
    {
        services
            .AddRazorLight(() => new RazorLightEngineBuilder()
                .UseFileSystemProject(AppDomain.CurrentDomain.BaseDirectory)
                .UseMemoryCachingProvider()
                .Build()
            );

        return services
            .AddScoped<ITemplateRepository, StaticHtmlTemplateFileRepository>()
            .AddScoped<IHtmlTemplateRenderer, RazorHtmlTemplateRenderer>();
    }

    private static IServiceCollection AddEmailSender(this IServiceCollection services)
    {
        services
            .AddOptions<SendGridSettings>()
            .BindConfiguration(SendGridSettings.ConfigurationName)
            .ValidateDataAnnotations();

        return services
            .AddScoped<IEmailSender, SendGridEmailSender>()
            .AddScoped<ISendGridClient>(sp =>
            {
                var settings = sp.GetRequiredService<IOptions<SendGridSettings>>();

                var sendGridSettings = settings.Value;

                return new SendGridClient(
                    sendGridSettings.ApiKey,
                    string.IsNullOrWhiteSpace(sendGridSettings.Host) ? null : sendGridSettings.Host
                );
            });
    }

    public static IServiceCollection ConfigureHtmlRenderingForTests(this IServiceCollection services)
    {
        // ISSUE : NCrunch do not inject all assemblies in the program, so
        // it is incompatible with Razor light engine.
        // The workaround is to provide metadata dependencies manually.
        // see here : https://forum.ncrunch.net/yaf_postst2968_Tests-using-razorlight-fail.aspx

        services.Remove(services.Single(x => x.ServiceType == typeof(IRazorLightEngine)));

        services.AddRazorLight(() => new RazorLightEngineBuilder()
            .UseFileSystemProject(AppDomain.CurrentDomain.BaseDirectory)
            .UseMemoryCachingProvider()
            .AddMetadataReferences(GetMetadataReferences())
            .Build()
        );

        return services;
    }

    private static MetadataReference[] GetMetadataReferences()
    {
        string[] assemblyLocations =
        {
            typeof(DynamicObject).Assembly.Location,
            Path.Combine(Path.GetDirectoryName(typeof(object).Assembly.Location)!, "System.Runtime.dll"),
            typeof(object).Assembly.Location, typeof(Enumerable).Assembly.Location,
            typeof(CSharpArgumentInfo).Assembly.Location
        };

        return assemblyLocations
            .Distinct()
            .Select(x => MetadataReference.CreateFromFile(x))
            .Cast<MetadataReference>()
            .ToArray();
    }
}
