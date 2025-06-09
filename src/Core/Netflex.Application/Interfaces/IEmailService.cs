namespace Netflex.Application.Interfaces;

public record EmailSettings(string Key, string OwnerMail, string Company);

public interface IEmailService
{
    EmailSettings Settings { get; }
    Task SendEmailAsync(string toEmail, string subject, string htmlContent, CancellationToken cancellationToken = default);
}