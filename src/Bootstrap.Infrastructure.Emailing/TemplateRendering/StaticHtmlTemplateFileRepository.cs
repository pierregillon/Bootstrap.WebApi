namespace Bootstrap.Infrastructure.Emailing.TemplateRendering;

public class StaticHtmlTemplateFileRepository : ITemplateRepository
{
    public Task<Template> GetWelcomeEmailTemplate()
        => Task.FromResult(new Template("TemplateRendering\\Templates\\welcome-email.cshtml"));
}
