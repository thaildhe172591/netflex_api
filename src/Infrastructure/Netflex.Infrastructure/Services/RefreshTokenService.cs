using Microsoft.Extensions.Options;
using Netflex.Application.Interfaces;

namespace Netflex.Infrastructure.Services;

public class RefreshOptions(IOptions<RefreshSettings> options) : IRefreshOptions
{
    private readonly RefreshSettings _settings = options.Value;
    public int ExpiresInDays => _settings.ExpiresInDays;
}