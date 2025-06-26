using FluentValidation;
using Netflex.Domain.Entities;
using Netflex.Shared.Exceptions;

namespace Netflex.Application.UseCases.V1.Movies.Commands;

public record UpdateMovieCommand(
    long Id,
    string? Title,
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
) : ICommand<UpdateMovieResult>;

public record UpdateMovieResult(long Id);

public class UpdateMovieCommandValidator : AbstractValidator<UpdateMovieCommand>
{
    public UpdateMovieCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}

public class UpdateMovieHandler(IUnitOfWork unitOfWork)
    : ICommandHandler<UpdateMovieCommand, UpdateMovieResult>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<UpdateMovieResult> Handle(UpdateMovieCommand request, CancellationToken cancellationToken)
    {
        var movieRepository = _unitOfWork.Repository<Movie>();
        var movie = await movieRepository
            .GetAsync(m => m.Id == request.Id, cancellationToken: cancellationToken)
            ?? throw new NotFoundException(nameof(Movie), request.Id);
        movie.Update(
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

        movieRepository.Update(movie);
        await _unitOfWork.CommitAsync();
        return new UpdateMovieResult(movie.Id);
    }
}