using Bootstrap.Infrastructure.Emailing.EmailDelivery;

namespace Bootstrap.Infrastructure.Emailing.TemplateRendering;

public interface IHtmlTemplateRenderer
{
    Task<HtmlContent> Render<T>(Template template, T model);
}
