using System.Data;
using Dapper;
using Netflex.Application.Dtos;
using Netflex.Application.Interfaces.Repositories.ReadOnly;
using Netflex.Shared.Pagination;

namespace Netflex.Persistence.Repositories.ReadOnly;

public class KeywordReadOnlyRepository : ReadOnlyRepository, IKeywordReadOnlyRepository
{
    public KeywordReadOnlyRepository(IDbConnection connection) : base(connection)
    {
        _columns = ["keyword_id", "name"];
    }

    public Task<PaginatedResult<KeywordDto>> GetKeywordsAsync(string? search, string? sortBy,
        int pageIndex, int pageSize, CancellationToken cancellationToken = default)
    {
        var query = @"
            SELECT  keyword_id as id, name
            FROM dbo.keywords
            WHERE name ILIKE @Search
        ";
        var parameters = new DynamicParameters();
        parameters.Add("Search", $"%{search}%");
        return GetPagedDataAsync<KeywordDto>(query, sortBy, pageIndex, pageSize, parameters);
    }
}