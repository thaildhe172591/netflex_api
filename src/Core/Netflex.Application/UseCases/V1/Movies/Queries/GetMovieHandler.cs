using Netflex.Application.Interfaces.Repositories.ReadOnly;
using Netflex.Domain.Entities;
using Netflex.Shared.Exceptions;
namespace Netflex.Application.UseCases.V1.Movies.Queries;

public record GetMovieQuery(long Id)
    : IQuery<MovieDto>;

public class GetMovieHandler(IMovieReadOnlyRepository movieReadOnlyRepository)
    : IQueryHandler<GetMovieQuery, MovieDto>
{
    private readonly IMovieReadOnlyRepository _movieReadOnlyRepository = movieReadOnlyRepository;
    public async Task<MovieDto> Handle(GetMovieQuery request, CancellationToken cancellationToken)
    {
        var result = await _movieReadOnlyRepository.GetMovieAsync(
            request.Id,
            cancellationToken
        ) ?? throw new NotFoundException(nameof(Movie), request.Id);
        return result;
    }
}