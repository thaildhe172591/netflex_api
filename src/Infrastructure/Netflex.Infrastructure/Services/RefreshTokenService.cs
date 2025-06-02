using Microsoft.Extensions.Options;
using Netflex.Application.Interfaces;

namespace Netflex.Infrastructure.Services;

public record RefreshConfig
{
    public int ExpiresInDays { get; init; }
};

public class RefreshTokenService(IOptions<RefreshConfig> options) : IRefreshTokenService
{
    private readonly RefreshConfig _config = options.Value;
    public int ExpiresInDays => _config.ExpiresInDays;
}