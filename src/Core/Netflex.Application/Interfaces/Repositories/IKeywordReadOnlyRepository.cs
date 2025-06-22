using Netflex.Shared.Pagination;

namespace Netflex.Application.Interfaces.Repositories;

public interface IKeywordReadOnlyRepository : IReadOnlyRepository
{
    public Task<PaginatedResult<KeywordDTO>> GetKeywordsAsync(string? search, string? sortBy,
        int page, int pageSize, CancellationToken cancellationToken = default);
}