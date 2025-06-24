using Netflex.Shared.Pagination;

namespace Netflex.Application.Interfaces.Repositories.ReadOnly;

public interface IKeywordReadOnlyRepository : IReadOnlyRepository
{
    public Task<PaginatedResult<KeywordDto>> GetKeywordsAsync(string? search, string? sortBy,
        int pageIndex, int pageSize, CancellationToken cancellationToken = default);
}