using FluentValidation;
using Netflex.Domain.Entities;
using Netflex.Shared.Exceptions;

namespace Netflex.Application.UseCases.V1.Movies.Commands;

public record UpdateMovieCommand(
    long Id,
    string? Title,
    string? Overview,
    IFileResource? Poster,
    IFileResource? Backdrop,
    IFileResource? Video,
    string? CountryIso,
    TimeSpan? RunTime,
    DateTime? ReleaseDate,
    ICollection<long>? Actors,
    ICollection<long>? Keywords,
    ICollection<long>? Genres
) : ICommand<UpdateMovieResult>;

public record UpdateMovieResult(long Id);

public class UpdateMovieCommandValidator : AbstractValidator<UpdateMovieCommand>
{
    public UpdateMovieCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}

public class UpdateMovieHandler(IUnitOfWork unitOfWork, ICloudStorage cloudStorage)
    : ICommandHandler<UpdateMovieCommand, UpdateMovieResult>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ICloudStorage _cloudStorage = cloudStorage;

    public async Task<UpdateMovieResult> Handle(UpdateMovieCommand request, CancellationToken cancellationToken)
    {
        var movieRepository = _unitOfWork.Repository<Movie>();
        var movie = await movieRepository.GetAsync(m => m.Id == request.Id,
            includeProperties: [nameof(Movie.Actors), nameof(Movie.Keywords), nameof(Movie.Genres)],
            cancellationToken: cancellationToken)
                ?? throw new NotFoundException(nameof(Movie), request.Id);

        Uri? poster = null, backdrop = null, video = null;
        var tasks = new List<Task>();

        if (request.Poster != null)
        {
            var task = _cloudStorage.UploadAsync("poster", request.Poster)
                .ContinueWith(t => poster = t.Result, cancellationToken);
            tasks.Add(task);
        }

        if (request.Backdrop != null)
        {
            var task = _cloudStorage.UploadAsync("backdrop", request.Backdrop)
                .ContinueWith(t => backdrop = t.Result, cancellationToken);
            tasks.Add(task);
        }

        if (request.Video != null)
        {
            var task = _cloudStorage.UploadAsync("video", request.Video)
                .ContinueWith(t => video = t.Result, cancellationToken);
            tasks.Add(task);
        }

        await Task.WhenAll(tasks);

        movie.Update(
            request.Title,
            request.Overview,
            poster?.ToString(),
            backdrop?.ToString(),
            request.RunTime,
            request.ReleaseDate,
            request.CountryIso,
            video?.ToString()
        );

        if (request.Actors != null)
        {
            var actors = request.Actors.Count != 0
                ? await _unitOfWork.Repository<Actor>()
                    .GetAllAsync(a => request.Actors.Contains(a.Id), cancellationToken: cancellationToken) : [];
            movie.AssignActors(actors);
        }

        if (request.Keywords != null)
        {
            var keywords = request.Keywords.Count != 0
                ? await _unitOfWork.Repository<Keyword>()
                    .GetAllAsync(k => request.Keywords.Contains(k.Id), cancellationToken: cancellationToken) : [];
            movie.AssignKeywords(keywords);
        }

        if (request.Genres != null)
        {
            var genres = request.Genres.Count != 0
                ? await _unitOfWork.Repository<Genre>()
                    .GetAllAsync(g => request.Genres.Contains(g.Id), cancellationToken: cancellationToken) : [];
            movie.AssignGenres(genres);
        }

        movieRepository.Update(movie);
        await _unitOfWork.CommitAsync();
        return new UpdateMovieResult(movie.Id);
    }
}