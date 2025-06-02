using Netflex.Application.Interfaces.Repositories;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace Netflex.Persistence.Repositories;

public class CachedUserRepository(ApplicationDbContext dbContext,
    IUserRepository repository, IDistributedCache cache)
    : BaseRepository<User>(dbContext), IUserRepository
{
    private readonly IUserRepository _repository = repository;
    private readonly IDistributedCache _cache = cache;
    private const int CACHE_EXPIRES_IN_MINUTES = 15;
    public async Task<int> GetVersionByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        var key = $"{nameof(User).ToLowerInvariant()}:{id}:{nameof(User.Version).ToLowerInvariant()}";
        var cached = await _cache.GetStringAsync(key, cancellationToken);
        if (!string.IsNullOrEmpty(cached))
            return JsonSerializer.Deserialize<int>(cached);
        var version = await _repository.GetVersionByIdAsync(id, cancellationToken);
        await _cache.SetStringAsync(key, JsonSerializer.Serialize(version), new()
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(CACHE_EXPIRES_IN_MINUTES)
        }, cancellationToken);
        return version;
    }

    public async Task ResetVersionByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        await _repository.ResetVersionByIdAsync(id, cancellationToken);
        var key = $"{nameof(User).ToLowerInvariant()}:{id}:{nameof(User.Version).ToLowerInvariant()}";
        await _cache.RemoveAsync(key, cancellationToken);
    }
}