namespace Netflex.Application.Interfaces;

public interface IOtpGenerator
{
    Task<string> GenerateOtpAsync(string email, CancellationToken cancellationToken = default);
    Task<bool> VerifyOtpAsync(string email, string otp, bool revoke = false, CancellationToken cancellationToken = default);
}