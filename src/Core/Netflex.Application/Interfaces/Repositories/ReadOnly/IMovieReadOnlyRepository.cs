using Netflex.Shared.Pagination;

namespace Netflex.Application.Interfaces.Repositories.ReadOnly;

public interface IMovieReadOnlyRepository : IReadOnlyRepository
{
    Task<MovieDto?> GetMovieAsync(long id, CancellationToken cancellationToken);
    Task<PaginatedResult<MovieDto>> GetMoviesAsync(
        string? search, IEnumerable<long>? keywordIds, IEnumerable<long>? genreIds, IEnumerable<long>? actorIds,
        string? sortBy, int pageIndex, int pageSize, CancellationToken cancellationToken = default);
}