using Netflex.Application.Interfaces;
using Netflex.Domain.ValueObjects;

namespace Netflex.Infrastructure.Services;

public class SocialServiceFactory(IEnumerable<ISocialService> services)
        : ISocialServiceFactory
{
    private readonly Dictionary<LoginProvider, ISocialService> _serviceByProvider =
        services.ToDictionary(s => s.Provider, s => s);

    public ISocialService? GetByProvider(LoginProvider provider)
        => _serviceByProvider.TryGetValue(provider, out var service) ? service : null;
}