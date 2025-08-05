using Microsoft.Extensions.Configuration;
using Netflex.Application.Common.Exceptions;
using Netflex.Application.Interfaces;
using Netflex.Infrastructure.Exceptions;
using System.Net;
using System.Net.Mail;

namespace Netflex.Infrastructure.Services;


public class EmailService(IConfiguration configuration) : IEmailService
{
    private readonly EmailSettings _settings = configuration.GetSection(nameof(EmailSettings)).Get<EmailSettings>()
            ?? throw new NotConfiguredException(nameof(EmailSettings));

    public EmailSettings Settings => _settings;

    public async Task SendEmailAsync(string toEmail, string subject,
        string htmlContent, CancellationToken cancellationToken = default)
    {
        try
        {
            using var client = new SmtpClient(_settings.Host, _settings.Port);

            if (_settings.UseAuthentication)
            {
                client.Credentials = new NetworkCredential(_settings.Username, _settings.Password);
            }

            client.EnableSsl = _settings.EnableSsl;

            using var message = new MailMessage
            {
                From = new MailAddress(_settings.OwnerMail, _settings.Company),
                Subject = subject,
                Body = htmlContent,
                IsBodyHtml = true
            };

            message.To.Add(toEmail);

            await client.SendMailAsync(message, cancellationToken);
        }
        catch (Exception ex)
        {
            throw new EmailNotSentException($"{subject} - {ex.Message}");
        }
    }
}