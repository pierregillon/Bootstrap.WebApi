namespace Bootstrap.Infrastructure.Emailing.TemplateRendering;

public interface ITemplateRepository
{
    Task<Template> GetWelcomeEmailTemplate();
}
