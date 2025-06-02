using Microsoft.Extensions.Caching.Distributed;
using Netflex.Application.Interfaces;

namespace Netflex.Infrastructure.Services;

public class OTPGenerator(IDistributedCache distributedCache) : IOTPGenerator
{
    private readonly IDistributedCache _distributedCache = distributedCache;
    private const int OTP_EPIRES_IN_MINUTES = 5;

    public async Task<string> GenerateOTPAsync(string email, CancellationToken cancellationToken = default)
    {
        var otp = new Random().Next(100000, 999999).ToString();
        await _distributedCache.SetStringAsync($"otp:{email}", otp, new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(OTP_EPIRES_IN_MINUTES)
        }, token: cancellationToken);

        return otp;
    }

    public async Task<bool> VerifyOTPAsync(string email, string otp, bool revoke = false, CancellationToken cancellationToken = default)
    {
        var cacheOTP = await _distributedCache.GetStringAsync($"otp:{email}", token: cancellationToken);
        var isValid = cacheOTP != null && cacheOTP == otp;
        if (revoke && isValid)
        {
            await _distributedCache.RemoveAsync($"otp:{email}", token: cancellationToken);
        }
        return isValid;
    }

}