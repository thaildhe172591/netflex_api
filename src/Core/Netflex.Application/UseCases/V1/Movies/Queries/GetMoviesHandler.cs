using Netflex.Application.Interfaces.Repositories.ReadOnly;
using Netflex.Shared.Pagination;

namespace Netflex.Application.UseCases.V1.Movies.Queries;

public record GetMoviesQuery(
    string? Search,
    IEnumerable<long>? KeywordIds,
    IEnumerable<long>? GenreIds,
    IEnumerable<long>? ActorIds,
    string? SortBy,
    int PageIndex,
    int PageSize
) : IQuery<PaginatedResult<MovieDto>>;

public class GetMoviesHandler(IMovieReadOnlyRepository movieReadOnlyRepository)
    : IQueryHandler<GetMoviesQuery, PaginatedResult<MovieDto>>
{
    private readonly IMovieReadOnlyRepository _movieReadOnlyRepository = movieReadOnlyRepository;
    public async Task<PaginatedResult<MovieDto>> Handle(GetMoviesQuery request, CancellationToken cancellationToken)
    {
        var result = await _movieReadOnlyRepository.GetMoviesAsync(
            request.Search,
            request.KeywordIds,
            request.GenreIds,
            request.ActorIds,
            request.SortBy,
            request.PageIndex,
            request.PageSize,
            cancellationToken
        );
        return result;
    }
}