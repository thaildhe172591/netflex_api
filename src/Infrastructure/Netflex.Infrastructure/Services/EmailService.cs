using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;
using Netflex.Application.Exceptions;
using Netflex.Application.Interfaces;

namespace Netflex.Infrastructure.Services;


public class EmailService(IConfiguration configuration) : IEmailService
{
    private readonly EmailConfig _config = configuration.GetSection(nameof(EmailConfig)).Get<EmailConfig>()
            ?? throw new NotConfiguredException(nameof(EmailConfig));

    public EmailConfig Config => _config;

    public async Task SendEmailAsync(string toEmail, string subject,
        string bodyHtml, CancellationToken cancellationToken = default)
    {
        var email = new MimeMessage
        {
            Sender = new MailboxAddress(_config.Company, _config.Username)
        };
        email.From.Add(new MailboxAddress(_config.Company, _config.Username));
        email.To.Add(MailboxAddress.Parse(toEmail));
        email.Subject = subject;

        var builder = new BodyBuilder { HtmlBody = bodyHtml };
        email.Body = builder.ToMessageBody();

        using var smtp = new SmtpClient();

        await smtp.ConnectAsync(_config.Host, _config.Port, MailKit.Security.SecureSocketOptions.StartTls, cancellationToken);
        await smtp.AuthenticateAsync(_config.Username, _config.Password, cancellationToken);
        await smtp.SendAsync(email);
        await smtp.DisconnectAsync(true, cancellationToken);
    }
}