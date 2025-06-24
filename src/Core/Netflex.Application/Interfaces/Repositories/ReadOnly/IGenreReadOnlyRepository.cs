using Netflex.Shared.Pagination;

namespace Netflex.Application.Interfaces.Repositories.ReadOnly;

public interface IGenreReadOnlyRepository : IReadOnlyRepository
{
    public Task<PaginatedResult<GenreDto>> GetGenresAsync(string? search, string? sortBy,
        int pageIndex, int pageSize, CancellationToken cancellationToken = default);
}