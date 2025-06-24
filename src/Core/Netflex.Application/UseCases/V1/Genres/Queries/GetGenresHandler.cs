using Netflex.Application.Interfaces.Repositories.ReadOnly;
using Netflex.Shared.Pagination;

namespace Netflex.Application.UseCases.V1.Genres.Queries;

public record GetGenresQuery(string? Search, string? SortBy, int PageIndex, int PageSize)
    : IQuery<PaginatedResult<GenreDto>>;

public class GetGenresHandler(IGenreReadOnlyRepository genreReadOnlyRepository)
    : IQueryHandler<GetGenresQuery, PaginatedResult<GenreDto>>
{
    private readonly IGenreReadOnlyRepository _genreReadOnlyRepository = genreReadOnlyRepository;
    public async Task<PaginatedResult<GenreDto>> Handle(GetGenresQuery request, CancellationToken cancellationToken)
    {
        var result = await _genreReadOnlyRepository.GetGenresAsync(
            request.Search,
            request.SortBy,
            request.PageIndex,
            request.PageSize,
            cancellationToken
        );
        return result;
    }
}