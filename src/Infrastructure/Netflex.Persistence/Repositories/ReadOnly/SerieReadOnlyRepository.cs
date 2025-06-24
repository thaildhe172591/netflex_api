using System.Data;
using System.Text;
using Dapper;
using Netflex.Application.Dtos;
using Netflex.Application.Interfaces.Repositories.ReadOnly;
using Netflex.Shared.Pagination;

namespace Netflex.Persistence.Repositories.ReadOnly;

public class SerieReadOnlyRepository : ReadOnlyRepository, ISerieReadOnlyRepository
{
    public SerieReadOnlyRepository(IDbConnection connection) : base(connection)
    {
        _columns = ["tv_serie_id", "name","overview", "poster_path",
            "backdrop_path", "country_iso", "first_air_date", "last_air_date"];
    }

    public async Task<SerieDto?> GetSerieAsync(long id, CancellationToken cancellationToken)
    {
        var sql = @"
            -- Serie
            SELECT 
                tv_serie_id AS id,
                name,
                overview,
                poster_path,
                backdrop_path,
                country_iso,
                first_air_date,
                last_air_date
            FROM dbo.tv_series
            WHERE tv_serie_id = @Id;

            -- Keywords
            SELECT 
                k.keyword_id AS id,
                k.name
            FROM dbo.keywords k
            INNER JOIN dbo.serie_keywords sk ON sk.keyword_id = k.keyword_id
            WHERE sk.serie_id = @Id;

            -- Genres
            SELECT 
                g.genre_id AS id,
                g.name
            FROM dbo.genres g
            INNER JOIN dbo.serie_genres sg ON sg.genre_id = g.genre_id
            WHERE sg.serie_id = @Id;
        ";

        using var multi = await _connection.QueryMultipleAsync(sql, new { Id = id });

        var serie = await multi.ReadSingleOrDefaultAsync<SerieDto>();
        if (serie == null) return null;

        var keywords = (await multi.ReadAsync<KeywordDto>()).ToList();
        var genres = (await multi.ReadAsync<GenreDto>()).ToList();

        return serie with
        {
            Keywords = keywords,
            Genres = genres
        };
    }


    public Task<PaginatedResult<SerieDto>> GetSeriesAsync(string? search,
        IEnumerable<long>? keywordIds, IEnumerable<long>? genreIds, string? sortBy,
        int pageIndex, int pageSize, CancellationToken cancellationToken = default)
    {
        var query = new StringBuilder(@"
            SELECT  serie_id as id, name, overview, poster_path, backdrop_path, country_iso, first_air_date, last_air_date
            FROM dbo.tv_series
            WHERE 1 = 1
        ");
        var parameters = new DynamicParameters();

        if (!string.IsNullOrWhiteSpace(search))
        {
            query.AppendLine("AND s.name ILIKE @Search");
            parameters.Add("Search", $"%{search}%");
        }

        if (genreIds?.Any() == true)
        {
            query.AppendLine(@"
                AND EXISTS (
                    SELECT 1 FROM dbo.serie_genres sg
                    INNER JOIN dbo.genres g ON g.genre_id = sg.genre_id
                    WHERE sg.serie_id = s.tv_serie_id
                    AND g.genre_id = ANY(@GenreIds)
                )
            ");
            parameters.Add("GenreIds", genreIds);
        }

        if (keywordIds?.Any() == true)
        {
            query.AppendLine(@"
                AND EXISTS (
                    SELECT 1 FROM dbo.serie_keywords sk
                    INNER JOIN dbo.keywords k ON k.keyword_id = sk.keyword_id
                    WHERE sk.serie_id = s.tv_serie_id
                    AND k.keyword_id = ANY(@KeywordIds)
                )
            ");
            parameters.Add("KeywordIds", keywordIds);
        }

        return GetPagedDataAsync<SerieDto>(
            query.ToString(),
            sortBy,
            pageIndex,
            pageSize,
            parameters);
    }
}