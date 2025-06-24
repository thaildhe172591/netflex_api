using System.Data;
using Dapper;
using Netflex.Application.Dtos;
using Netflex.Application.Interfaces.Repositories.ReadOnly;
using Netflex.Shared.Pagination;

namespace Netflex.Persistence.Repositories.ReadOnly;

public class GenreReadOnlyRepository : ReadOnlyRepository, IGenreReadOnlyRepository
{
    public GenreReadOnlyRepository(IDbConnection connection) : base(connection)
    {
        _columns = ["genre_id", "name"];
    }

    public Task<PaginatedResult<GenreDto>> GetGenresAsync(string? search, string? sortBy,
        int pageIndex, int pageSize, CancellationToken cancellationToken = default)
    {
        var query = @"
            SELECT genre_id as id, name
            FROM dbo.genres
            WHERE name ILIKE @Search
        ";
        var parameters = new DynamicParameters();
        parameters.Add("Search", $"%{search}%");
        return GetPagedDataAsync<GenreDto>(query, sortBy, pageIndex, pageSize, parameters);
    }
}