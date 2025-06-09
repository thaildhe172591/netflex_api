using Microsoft.Extensions.Options;
using Netflex.Application.Interfaces;

namespace Netflex.Infrastructure.Services;

public record RefreshSettings
{
    public int ExpiresInDays { get; init; }
};

public class RefreshTokenService(IOptions<RefreshSettings> options) : IRefreshTokenService
{
    private readonly RefreshSettings _settings = options.Value;
    public int ExpiresInDays => _settings.ExpiresInDays;
}