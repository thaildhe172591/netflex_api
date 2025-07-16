using Netflex.Shared.Pagination;

namespace Netflex.Application.Interfaces.Repositories.ReadOnly;

public interface IMovieReadOnlyRepository : IReadOnlyRepository
{
    Task<MovieDetailDto?> GetMovieAsync(long id, CancellationToken cancellationToken);
    Task<PaginatedResult<MovieDto>> GetMoviesAsync(
        string? search, IEnumerable<long>? keywordIds, IEnumerable<long>? genreIds, IEnumerable<long>? actorIds,
        string? country, int? year, string? sortBy, string? followerId, int pageIndex, int pageSize, CancellationToken cancellationToken = default);
}