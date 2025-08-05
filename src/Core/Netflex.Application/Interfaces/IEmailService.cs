namespace Netflex.Application.Interfaces;

public record EmailSettings(
    string Host,
    int Port,
    string Username,
    string Password,
    string OwnerMail,
    string Company,
    bool EnableSsl = true,
    bool UseAuthentication = true);

public interface IEmailService
{
    EmailSettings Settings { get; }
    Task SendEmailAsync(string toEmail, string subject, string htmlContent, CancellationToken cancellationToken = default);
}