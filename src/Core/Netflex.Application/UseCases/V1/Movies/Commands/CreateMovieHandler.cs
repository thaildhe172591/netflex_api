using FluentValidation;
using Netflex.Domain.Entities;

namespace Netflex.Application.UseCases.V1.Movies.Commands;

public record CreateMovieCommand(
    string Title,
    string? Overview,
    string? PosterPath,
    string? BackdropPath,
    string? VideoUrl,
    string? CountryIso,
    TimeSpan? RunTime,
    DateTime? ReleaseDate,
    ICollection<long>? ActorIds,
    ICollection<long>? KeywordIds,
    ICollection<long>? GenreIds
) : ICommand<CreateMovieResult>;

public record CreateMovieResult(long Id);
public class CreateMovieCommandValidator : AbstractValidator<CreateMovieCommand>
{
    public CreateMovieCommandValidator()
    {
        RuleFor(x => x.Title).NotEmpty();
    }
}

public class CreateMovieHandler(IUnitOfWork unitOfWork)
    : ICommandHandler<CreateMovieCommand, CreateMovieResult>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<CreateMovieResult> Handle(CreateMovieCommand request, CancellationToken cancellationToken)
    {
        var movie = Movie.Create(
            request.Title,
            request.Overview,
            request.PosterPath,
            request.BackdropPath,
            request.RunTime,
            request.ReleaseDate,
            request.CountryIso,
            request.VideoUrl
        );

        if (request.ActorIds != null)
        {
            var actors = request.ActorIds.Count != 0
                ? await _unitOfWork.Repository<Actor>()
                    .GetAllAsync(a => request.ActorIds.Contains(a.Id), cancellationToken: cancellationToken) : [];
            movie.AssignActors(actors);
        }

        if (request.KeywordIds != null)
        {
            var keywords = request.KeywordIds.Count != 0
                ? await _unitOfWork.Repository<Keyword>()
                    .GetAllAsync(k => request.KeywordIds.Contains(k.Id), cancellationToken: cancellationToken) : [];
            movie.AssignKeywords(keywords);
        }

        if (request.GenreIds != null)
        {
            var genres = request.GenreIds.Count != 0
                ? await _unitOfWork.Repository<Genre>()
                    .GetAllAsync(g => request.GenreIds.Contains(g.Id), cancellationToken: cancellationToken) : [];
            movie.AssignGenres(genres);
        }

        await _unitOfWork.Repository<Movie>().AddAsync(movie, cancellationToken: cancellationToken);
        await _unitOfWork.CommitAsync();
        return new CreateMovieResult(movie.Id);
    }
}
