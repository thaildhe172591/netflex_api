using Microsoft.Extensions.Caching.Distributed;
using Netflex.Application.Interfaces;

namespace Netflex.Infrastructure.Services;

public class OtpGenerator(IDistributedCache distributedCache) : IOtpGenerator
{
    private readonly IDistributedCache _distributedCache = distributedCache;
    private const int OTP_EPIRES_IN_MINUTES = 5;

    public async Task<string> GenerateOtpAsync(string email, CancellationToken cancellationToken = default)
    {
        var otp = new Random().Next(100000, 999999).ToString();
        await _distributedCache.SetStringAsync($"otp:{email}", otp, new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(OTP_EPIRES_IN_MINUTES)
        }, token: cancellationToken);

        return otp;
    }

    public async Task<bool> VerifyOtpAsync(string email, string otp, bool revoke = false, CancellationToken cancellationToken = default)
    {
        var cacheOtp = await _distributedCache.GetStringAsync($"otp:{email}", token: cancellationToken);
        var isValid = cacheOtp != null && cacheOtp == otp;
        if (revoke && isValid)
        {
            await _distributedCache.RemoveAsync($"otp:{email}", token: cancellationToken);
        }
        return isValid;
    }

}