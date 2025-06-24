using System.Data;
using Dapper;
using Netflex.Application.Interfaces.Repositories.ReadOnly;
using Netflex.Shared.Pagination;

namespace Netflex.Persistence.Repositories.ReadOnly;

public class ReadOnlyRepository(IDbConnection connection) : IReadOnlyRepository
{
    protected readonly IDbConnection _connection = connection;
    protected IEnumerable<string> _columns = [];

    protected async Task<PaginatedResult<T>> GetPagedDataAsync<T>(string query, string? sortBy,
        int pageIndex, int pageSize, DynamicParameters? parameters = null) where T : class
    {
        var offset = (pageIndex - 1) * pageSize;
        var orderBy = ParseOrderBy(sortBy);

        var pagedQuery = @$"
            {query}
            ORDER BY {orderBy}
            LIMIT @PageSize OFFSET @Offset";

        var countQuery = @$"SELECT COUNT(*) FROM ({query}) AS Total";
        parameters ??= new DynamicParameters();
        parameters.Add("@Offset", offset);
        parameters.Add("@PageSize", pageSize);

        using var multi = await _connection.QueryMultipleAsync(
            $"{countQuery};{pagedQuery}",
            parameters
        );

        int totalCount = await multi.ReadSingleAsync<int>();
        var data = await multi.ReadAsync<T>();

        return new PaginatedResult<T>(data, totalCount, pageIndex, pageSize);
    }

    private string ParseOrderBy(string? orderBy)
    {
        if (string.IsNullOrWhiteSpace(orderBy)) return "1";
        var orderByParts = orderBy.Split(',', StringSplitOptions.TrimEntries)
            .Select(part =>
            {
                var segments = part.Split('.', StringSplitOptions.TrimEntries);
                if (segments.Length != 2)
                    return null;

                var column = segments[0].ToLower();
                var direction = segments[1].ToLower() ?? "asc";

                if (!_columns.Contains(column) || direction != "asc" && direction != "desc")
                    return null;

                return $"{column} {direction.ToUpper()}";
            })
            .Where(part => part != null);

        var normalized = string.Join(", ", orderByParts);
        return string.IsNullOrWhiteSpace(normalized) ? "1" : normalized;
    }
}