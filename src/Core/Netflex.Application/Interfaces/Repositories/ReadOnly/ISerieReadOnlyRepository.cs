using Netflex.Shared.Pagination;

namespace Netflex.Application.Interfaces.Repositories.ReadOnly;

public interface ISerieReadOnlyRepository : IReadOnlyRepository
{
    Task<SerieDto?> GetSerieAsync(long id, CancellationToken cancellationToken);
    Task<PaginatedResult<SerieDto>> GetSeriesAsync(string? search, IEnumerable<long>? keywordIds, IEnumerable<long>? genreIds,
        string? sortBy, int pageIndex, int pageSize, CancellationToken cancellationToken = default);
}