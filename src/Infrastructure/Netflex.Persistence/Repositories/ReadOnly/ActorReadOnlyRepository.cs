using System.Data;
using Dapper;
using Netflex.Application.DTOs;
using Netflex.Application.Interfaces.Repositories.ReadOnly;
using Netflex.Shared.Pagination;

namespace Netflex.Persistence.Repositories.ReadOnly;

public class ActorReadOnlyRepository : ReadOnlyRepository, IActorReadOnlyRepository
{
    public ActorReadOnlyRepository(IDbConnection connection) : base(connection)
    {
        _columns = ["actor_id", "name", "image", "gender", "birth_date", "biography"];
    }

    public Task<PaginatedResult<ActorDto>> GetActorsAsync(string? search, string? sortBy, int pageIndex, int pageSize, CancellationToken cancellationToken = default)
    {
        var query = @"
            SELECT 
                actor_id as id, 
                name, image, gender, 
                birth_date as birthdate, 
                biography
            FROM dbo.actors a
            WHERE a.name ILIKE @Search
        ";

        var parameters = new DynamicParameters();
        parameters.Add("Search", $"%{search}%");
        return GetPagedDataAsync<ActorDto>(query, sortBy, pageIndex, pageSize, parameters);
    }
    public async Task<ActorDto?> GetByIdAsync(long id, CancellationToken cancellationToken = default)
    {
        var query = @"
            SELECT
                actor_id as id,
                name, image, gender,
                birth_date as birthdate,
                biography
            FROM dbo.actors a
            WHERE a.actor_id = @Id
        ";

        var parameters = new DynamicParameters();
        parameters.Add("Id", id);
        return await _connection.QuerySingleOrDefaultAsync<ActorDto>(query, parameters);
    }
}
