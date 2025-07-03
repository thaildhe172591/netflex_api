using FluentValidation;
using Netflex.Application.Extensions;
using Netflex.Domain.Entities;

namespace Netflex.Application.UseCases.V1.Series.Commands;

public record CreateSerieCommand(
    string Name,
    string? Overview,
    IFileResource? Poster,
    IFileResource? Backdrop,
    string? CountryIso,
    DateTime? FirstAirDate,
    DateTime? LastAirDate,
    ICollection<long>? Keywords,
    ICollection<long>? Genres
) : ICommand<CreateSerieResult>;

public record CreateSerieResult(long Id);
public class CreateSerieCommandValidator : AbstractValidator<CreateSerieCommand>
{
    public CreateSerieCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.Poster).MaxFileSize(5).AllowedExtensions(".jpg", ".jpeg", ".png", ".webp");
        RuleFor(x => x.Backdrop).MaxFileSize(5).AllowedExtensions(".jpg", ".jpeg", ".png", ".webp");
    }
}

public class CreateSerieHandler(IUnitOfWork unitOfWork, ICloudStorage cloudStorage)
    : ICommandHandler<CreateSerieCommand, CreateSerieResult>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ICloudStorage _cloudStorage = cloudStorage;

    public async Task<CreateSerieResult> Handle(CreateSerieCommand request, CancellationToken cancellationToken)
    {

        Uri? poster = null, backdrop = null;
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

        await Task.WhenAll(tasks);
        var serie = TVSerie.Create(
            request.Name,
            request.Overview,
            poster?.ToString(),
            backdrop?.ToString(),
            request.FirstAirDate,
            request.LastAirDate,
            request.CountryIso
        );

        if (request.Keywords != null)
        {
            var keywords = request.Keywords.Count != 0
                ? await _unitOfWork.Repository<Keyword>()
                    .GetAllAsync(k => request.Keywords.Contains(k.Id), cancellationToken: cancellationToken) : [];
            serie.AssignKeywords(keywords);
        }

        if (request.Genres != null)
        {
            var genres = request.Genres.Count != 0
                ? await _unitOfWork.Repository<Genre>()
                    .GetAllAsync(g => request.Genres.Contains(g.Id), cancellationToken: cancellationToken) : [];
            serie.AssignGenres(genres);
        }

        await _unitOfWork.Repository<TVSerie>().AddAsync(serie, cancellationToken: cancellationToken);
        await _unitOfWork.CommitAsync();
        return new CreateSerieResult(serie.Id);
    }
}