using Microsoft.Extensions.Configuration;
using Netflex.Application.Exceptions;
using Netflex.Application.Interfaces;
using Netflex.Infrastructure.Exceptions;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace Netflex.Infrastructure.Services;


public class EmailService(IConfiguration configuration) : IEmailService
{
    private readonly EmailSettings _settings = configuration.GetSection(nameof(EmailSettings)).Get<EmailSettings>()
            ?? throw new NotConfiguredException(nameof(EmailSettings));

    public EmailSettings Settings => _settings;

    public async Task SendEmailAsync(string toEmail, string subject,
        string html, CancellationToken cancellationToken = default)
    {
        var client = new SendGridClient(_settings.Key);
        var from = new EmailAddress(_settings.OwnerMail, _settings.Company);
        var to = new EmailAddress(toEmail);
        var msg = MailHelper.CreateSingleEmail(from, to, subject, null, html);
        var response = await client.SendEmailAsync(msg, cancellationToken);
        if (!response.IsSuccessStatusCode) throw new EmailNotSentException(subject);
    }
}