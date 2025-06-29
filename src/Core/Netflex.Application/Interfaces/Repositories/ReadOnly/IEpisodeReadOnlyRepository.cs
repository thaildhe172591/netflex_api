using Netflex.Shared.Pagination;

namespace Netflex.Application.Interfaces.Repositories.ReadOnly;

public interface IEpisodeReadOnlyRepository : IReadOnlyRepository
{
    Task<EpisodeDetailDto?> GetEpisodeAsync(long id, CancellationToken cancellationToken);
    Task<PaginatedResult<EpisodeDto>> GetEpisodesAsync(
        string? search, long? seriesId, IEnumerable<long>? actorIds, string? sortBy, int pageIndex, int pageSize, CancellationToken cancellationToken = default);
}
