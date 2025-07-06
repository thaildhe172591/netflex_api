using FluentValidation;
using Netflex.Application.Extensions;
using Netflex.Domain.Entities;

namespace Netflex.Application.UseCases.V1.Movies.Commands;

public record CreateMovieCommand(
    string Title,
    string? Overview,
    IFileResource? Poster,
    IFileResource? Backdrop,
    IFileResource? Video,
    string? CountryIso,
    int? Runtime,
    DateOnly? ReleaseDate,
    ICollection<long>? Actors,
    ICollection<long>? Keywords,
    ICollection<long>? Genres
) : ICommand<CreateMovieResult>;

public record CreateMovieResult(long Id);
public class CreateMovieCommandValidator : AbstractValidator<CreateMovieCommand>
{
    public CreateMovieCommandValidator()
    {
        RuleFor(x => x.Title).NotEmpty();
        RuleFor(x => x.Poster).MaxFileSize(5).AllowedExtensions(".jpg", ".jpeg", ".png", ".webp");
        RuleFor(x => x.Backdrop).MaxFileSize(5).AllowedExtensions(".jpg", ".jpeg", ".png", ".webp");
        RuleFor(x => x.Video).MaxFileSize(5).AllowedExtensions(".m3u8");
    }
}

public class CreateMovieHandler(IUnitOfWork unitOfWork, ICloudStorage cloudStorage)
    : ICommandHandler<CreateMovieCommand, CreateMovieResult>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ICloudStorage _cloudStorage = cloudStorage;

    public async Task<CreateMovieResult> Handle(CreateMovieCommand request, CancellationToken cancellationToken)
    {
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

        var movie = Movie.Create(
            request.Title,
            request.Overview,
            poster?.ToString(),
            backdrop?.ToString(),
            request.Runtime,
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

        await _unitOfWork.Repository<Movie>().AddAsync(movie, cancellationToken: cancellationToken);
        await _unitOfWork.CommitAsync();
        return new CreateMovieResult(movie.Id);
    }
}
