using Bootstrap.Infrastructure.Emailing.EmailDelivery;
using RazorLight;

namespace Bootstrap.Infrastructure.Emailing.TemplateRendering.RazorLightLib;

public class RazorHtmlTemplateRenderer : IHtmlTemplateRenderer
{
    private readonly IRazorLightEngine _engine;

    public RazorHtmlTemplateRenderer(IRazorLightEngine engine) => _engine = engine;

    public async Task<HtmlContent> Render<T>(Template template, T model)
    {
        var result = await _engine.CompileRenderAsync(template.RelativeFilePath, model);

        if (result is null)
        {
            throw new InvalidOperationException("Fail to renter template");
        }

        return new HtmlContent(result);
    }
}
