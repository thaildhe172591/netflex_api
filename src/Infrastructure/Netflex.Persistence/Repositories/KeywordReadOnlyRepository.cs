using System.Data;
using Netflex.Application.DTOs;
using Netflex.Application.Interfaces.Repositories;
using Netflex.Shared.Pagination;

namespace Netflex.Persistence.Repositories;

public class KeywordReadOnlyRepository : ReadOnlyRepository, IKeywordReadOnlyRepository
{
    public KeywordReadOnlyRepository(IDbConnection connection) : base(connection)
    {
        _columns = ["keyword_id", "name"];
    }

    public Task<PaginatedResult<KeywordDTO>> GetKeywordsAsync(string? search, string? sortBy,
        int page, int pageSize, CancellationToken cancellationToken = default)
    {

        throw new NotImplementedException();
    }
}