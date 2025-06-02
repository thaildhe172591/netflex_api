namespace Netflex.Application.Interfaces;

public interface IOTPGenerator
{
    Task<string> GenerateOTPAsync(string email, CancellationToken cancellationToken = default);
    Task<bool> VerifyOTPAsync(string email, string otp, bool revoke = false, CancellationToken cancellationToken = default);
}