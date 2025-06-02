namespace Netflex.Application.Interfaces;

public record EmailConfig(string Host, int Port, string Company,
    string Username, string Password);

public interface IEmailService
{
    EmailConfig Config { get; }
    Task SendEmailAsync(string toEmail, string subject, string htmlContent, CancellationToken cancellationToken = default);
}