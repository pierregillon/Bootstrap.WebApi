using Bootstrap.BuildingBlocks.DomainEvents;
using Bootstrap.Domain.Users;
using Bootstrap.Infrastructure.Emailing.EmailDelivery;
using Bootstrap.Infrastructure.Emailing.TemplateRendering;
using Bootstrap.Infrastructure.Emailing.TemplateRendering.Templates;
using Microsoft.Extensions.Logging;

namespace Bootstrap.Application.Users;

internal class SendWelcomeEmailOnUserRegistered : IDomainEventListener<UserRegistered>
{
    private readonly IEmailSender _emailSender;
    private readonly IHtmlTemplateRenderer _htmlTemplateRenderer;
    private readonly ILogger<SendWelcomeEmailOnUserRegistered> _logger;
    private readonly ITemplateRepository _templateRepository;

    public SendWelcomeEmailOnUserRegistered(
        ITemplateRepository templateRepository,
        IHtmlTemplateRenderer htmlTemplateRenderer,
        IEmailSender emailSender,
        ILogger<SendWelcomeEmailOnUserRegistered> logger)
    {
        _templateRepository = templateRepository;
        _htmlTemplateRenderer = htmlTemplateRenderer;
        _emailSender = emailSender;
        _logger = logger;
    }

    public async Task On(UserRegistered domainEvent)
    {
        try
        {
            var template = await _templateRepository.GetWelcomeEmailTemplate();

            var htmlContent = await _htmlTemplateRenderer.Render(template,
                new WelcomeEmailTemplate(domainEvent.EmailAddress));

            var email = new Email(
                EmailSender.SupportTeam,
                domainEvent.EmailAddress,
                new EmailSubject("Welcome to the app"),
                htmlContent
            );

            await _emailSender.Send(email);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Enable to send welcome email to user {0}", (string)domainEvent.EmailAddress);
        }
    }
}
