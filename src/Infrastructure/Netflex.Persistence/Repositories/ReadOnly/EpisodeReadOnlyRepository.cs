using System.Data;
using System.Text;
using Dapper;
using Netflex.Application.DTOs;
using Netflex.Application.Interfaces.Repositories.ReadOnly;
using Netflex.Shared.Pagination;

namespace Netflex.Persistence.Repositories.ReadOnly;

public class EpisodeReadOnlyRepository : ReadOnlyRepository, IEpisodeReadOnlyRepository
{
    public EpisodeReadOnlyRepository(IDbConnection connection) : base(connection)
    {
        _columns = ["episode_id", "name", "episode_number", "series_id",
            "overview", "video_url", "runtime", "air_date"];
    }

    public async Task<EpisodeDetailDto?> GetEpisodeAsync(long id, CancellationToken cancellationToken)
    {
        var sql = @"
            -- Episode
            SELECT 
                e.episode_id AS id, 
                e.name, 
                e.episode_number AS episodenumber, 
                e.overview, 
                e.video_url AS videourl, 
                e.runtime, 
                e.air_date AS airdate,
                e.series_id AS seriesid,
                s.name AS seriesname,
                s.poster_path AS posterpath
            FROM dbo.episodes e
            INNER JOIN dbo.tv_series s ON e.series_id = s.tv_serie_id
            WHERE e.episode_id = @Id;

            -- Actors
            SELECT 
                a.actor_id AS id, 
                a.name, 
                a.image
            FROM dbo.actors a
            INNER JOIN dbo.episode_actors ea ON a.actor_id = ea.actor_id
            WHERE ea.episode_id = @Id
        ";

        using var multi = await _connection.QueryMultipleAsync(sql, new { Id = id });

        var episode = await multi.ReadSingleOrDefaultAsync<EpisodeDetailDto>();
        if (episode == null) return null;

        var actors = (await multi.ReadAsync<ActorDto>()).ToList();

        return episode with
        {
            Actors = actors
        };
    }

    public Task<PaginatedResult<EpisodeDto>> GetEpisodesAsync(string? search, long? seriesId, IEnumerable<long>? actorIds, string? sortBy,
        int pageIndex, int pageSize, CancellationToken cancellationToken = default)
    {
        var query = new StringBuilder(@"
            SELECT 
                e.episode_id AS id, 
                e.name, 
                e.episode_number AS episodenumber, 
                e.overview, 
                e.video_url AS videourl, 
                e.runtime, 
                e.air_date AS airdate,
                e.series_id AS seriesid,
                s.name AS seriesname,
                s.poster_path AS posterpath
            FROM dbo.episodes e
            INNER JOIN dbo.tv_series s ON e.series_id = s.tv_serie_id
            WHERE 1 = 1
        ");
        var parameters = new DynamicParameters();
        if (!string.IsNullOrWhiteSpace(search))
        {
            query.AppendLine("AND e.name ILIKE @Search");
            parameters.Add("Search", $"%{search}%");
        }

        if (seriesId.HasValue)
        {
            query.AppendLine("AND e.series_id = @SeriesId");
            parameters.Add("SeriesId", seriesId.Value);
        }

        if (actorIds?.Any() == true)
        {
            query.AppendLine(@"
                AND EXISTS (
                    SELECT 1 FROM dbo.episode_actors ea
                    WHERE ea.episode_id = e.episode_id
                    AND ea.actor_id = ANY(@ActorIds)
                )
            ");
            parameters.Add("ActorIds", actorIds);
        }

        return GetPagedDataAsync<EpisodeDto>(query.ToString(), sortBy, pageIndex, pageSize, parameters);
    }
}