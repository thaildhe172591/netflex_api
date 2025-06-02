using Netflex.Application.Interfaces;
using Netflex.Domain.ValueObjects;

namespace Netflex.Infrastructure.Services;

public class SocialLoginServiceFactory(IEnumerable<ISocialLoginService> services)
        : ISocialLoginServiceFactory
{
    private readonly Dictionary<LoginProvider, ISocialLoginService> _serviceByProvider =
        services.ToDictionary(s => s.Provider, s => s);

    public ISocialLoginService? GetByProvider(LoginProvider provider)
        => _serviceByProvider.TryGetValue(provider, out var service) ? service : null;
}