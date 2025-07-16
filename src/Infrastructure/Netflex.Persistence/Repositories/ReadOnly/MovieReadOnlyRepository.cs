using System.Data;
using System.Text;
using Dapper;
using Netflex.Application.DTOs;
using Netflex.Application.Interfaces.Repositories.ReadOnly;
using Netflex.Shared.Pagination;

namespace Netflex.Persistence.Repositories.ReadOnly;

public class MovieReadOnlyRepository : ReadOnlyRepository, IMovieReadOnlyRepository
{
    public MovieReadOnlyRepository(IDbConnection connection) : base(connection)
    {
        _columns = ["movie_id", "title", "overview", "poster_path", "backdrop_path",
            "video_url", "country_iso", "run_time", "release_date"];
    }

    public async Task<MovieDetailDto?> GetMovieAsync(long id, CancellationToken cancellationToken)
    {
        var sql = @"
            -- Movie
            SELECT 
                movie_id AS id, 
                title, 
                overview, 
                poster_path AS posterpath, 
                backdrop_path AS backdroppath, 
                video_url AS videourl, 
                country_iso AS countryiso, 
                runtime, 
                release_date AS releasedate 
            FROM dbo.movies
            WHERE movie_id = @Id;

            -- Actors
            SELECT 
                a.actor_id AS id, 
                a.name, 
                a.image
            FROM dbo.actors a
            INNER JOIN dbo.movie_actors ma ON ma.actor_id = a.actor_id
            WHERE ma.movie_id = @Id;

            -- Keywords
            SELECT 
                k.keyword_id AS id, 
                k.name
            FROM dbo.keywords k
            INNER JOIN dbo.movie_keywords mk ON mk.keyword_id = k.keyword_id
            WHERE mk.movie_id = @Id;

            -- Genres
            SELECT 
                g.genre_id AS id, 
                g.name
            FROM dbo.genres g
            INNER JOIN dbo.movie_genres mg ON mg.genre_id = g.genre_id
            WHERE mg.movie_id = @Id;

            -- Rating
            SELECT 
                ROUND(AVG(rating::numeric), 1) AS averagerating,
                COUNT(*) AS totalreview
            FROM dbo.reviews
            WHERE target_id = CAST(@Id AS text) AND target_type = 'movie';
        ";

        using var multi = await _connection.QueryMultipleAsync(sql, new { Id = id });

        var movie = await multi.ReadSingleOrDefaultAsync<MovieDetailDto>();
        if (movie == null) return null;

        var actors = (await multi.ReadAsync<ActorDto>()).ToList();
        var keywords = (await multi.ReadAsync<KeywordDto>()).ToList();
        var genres = (await multi.ReadAsync<KeywordDto>()).ToList();
        var (avgRating, totalReviews) = await multi.ReadSingleOrDefaultAsync<(decimal?, int)>();

        return movie with
        {
            Actors = actors,
            Keywords = keywords,
            Genres = genres,
            AverageRating = avgRating,
            TotalReviews = totalReviews
        };
    }


    public Task<PaginatedResult<MovieDto>> GetMoviesAsync(string? search,
        IEnumerable<long>? keywordIds, IEnumerable<long>? genreIds, IEnumerable<long>? actorIds,
        string? country, int? year, string? sortBy, string? followerId, int pageIndex, int pageSize,
        CancellationToken cancellationToken = default)
    {
        var query = new StringBuilder(@"
            SELECT  
                m.movie_id AS id, 
                m.title, 
                m.overview, 
                m.poster_path AS posterpath, 
                m.backdrop_path AS backdroppath, 
                m.video_url AS videourl, 
                m.country_iso AS countryiso, 
                m.runtime, 
                m.release_date AS releasedate 
            FROM dbo.movies m
            WHERE 1 = 1
        ");
        var parameters = new DynamicParameters();
        if (!string.IsNullOrWhiteSpace(search))
        {
            query.AppendLine("AND m.title ILIKE @Search");
            parameters.Add("Search", $"%{search}%");
        }

        if (actorIds?.Any() == true)
        {
            query.AppendLine(@"
                AND EXISTS (
                    SELECT 1 FROM dbo.movie_actors ma
                    WHERE ma.movie_id = m.movie_id
                    AND ma.actor_id = ANY(@ActorIds)
                )
            ");
            parameters.Add("ActorIds", actorIds);
        }

        if (genreIds?.Any() == true)
        {
            query.AppendLine(@"
                AND EXISTS (
                    SELECT 1 FROM dbo.movie_genres mg
                    WHERE mg.movie_id = m.movie_id
                    AND mg.genre_id = ANY(@GenreIds)
                )
            ");
            parameters.Add("GenreIds", genreIds);
        }

        if (keywordIds?.Any() == true)
        {
            query.AppendLine(@"
                AND EXISTS (
                    SELECT 1 FROM dbo.movie_keywords mk
                    WHERE mk.movie_id = m.movie_id
                    AND mk.keyword_id = ANY(@KeywordIds)
                )
            ");
            parameters.Add("KeywordIds", keywordIds);
        }

        if (!string.IsNullOrWhiteSpace(country))
        {
            query.AppendLine("AND m.country_iso = @Country");
            parameters.Add("Country", country);
        }

        if (year.HasValue)
        {
            query.AppendLine("AND EXTRACT(YEAR FROM m.release_date) = @Year");
            parameters.Add("Year", year.Value);
        }

        if (!string.IsNullOrWhiteSpace(followerId))
        {
            query.AppendLine(@"
                AND EXISTS (
                    SELECT 1 FROM dbo.follows f
                    WHERE f.target_id = CAST(m.movie_id AS text)
                    AND f.target_type = 'movie'
                    AND f.user_id = @FollowerId
                )
            ");
            parameters.Add("FollowerId", followerId);
        }

        return GetPagedDataAsync<MovieDto>(query.ToString(), sortBy, pageIndex, pageSize, parameters);
    }
}