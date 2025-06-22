using System.Data;
using Dapper;
using Netflex.Application.Interfaces.Repositories;
using Netflex.Shared.Pagination;

namespace Netflex.Persistence.Repositories;

public class ReadOnlyRepository(IDbConnection connection) : IReadOnlyRepository
{
    protected readonly IDbConnection _connection = connection;
    protected IEnumerable<string> _columns = [];

    protected async Task<PaginatedResult<T>> GetPagedDataAsync<T>(string query, string? sortBy,
        int pageIndex, int pageSize, object? parameters = null) where T : class
    {
        var offset = (pageIndex - 1) * pageSize;
        var orderBy = GenerateOrderBy(sortBy);

        string pagedQuery = @$"
            {query}
            ORDER BY {orderBy}
            LIMIT @PageSize OFFSET @Offset";

        var countQuery = @$"SELECT COUNT(*) FROM ({query}) AS Total";
        var @params = new DynamicParameters(parameters);
        @params.Add("@Offset", offset);
        @params.Add("@PageSize", pageSize);

        using var multi = await _connection.QueryMultipleAsync(
            $"{countQuery};{pagedQuery}",
            @params
        );

        int totalCount = await multi.ReadSingleAsync<int>();
        var data = await multi.ReadAsync<T>();

        return new PaginatedResult<T>(data, totalCount, pageIndex, pageSize);
    }

    private string GenerateOrderBy(string? orderBy)
    {
        if (string.IsNullOrWhiteSpace(orderBy)) return "1";
        var orderByParts = orderBy.Split(',', StringSplitOptions.TrimEntries)
            .Select(part =>
            {
                var segments = part.Split('.', StringSplitOptions.TrimEntries);
                if (segments.Length != 2)
                    return null;

                var column = segments[0].ToLower();
                var direction = segments[1].ToLower();

                if (!_columns.Contains(column) || (direction != "asc" && direction != "desc"))
                    return null;

                return $"{column} {direction.ToUpper()}";
            })
            .Where(part => part != null);

        var normalized = string.Join(", ", orderByParts);
        return normalized ?? "1";
    }
}