using Netflex.Domain.Entities;
using FluentValidation;

namespace Netflex.Application.UseCases.V1.Episodes.Commands;

public record CreateEpisodeCommand(
    string Name,
    int EpisodeNumber,
    long SeriesId,
    string? Overview,
    IFileResource? Video,
    TimeSpan? Runtime,
    DateTime? AirDate,
    ICollection<long>? Actors
) : IRequest<CreateEpisodeResult>;


public class CreateEpisodeCommandValidator : AbstractValidator<CreateEpisodeCommand>
{
    public CreateEpisodeCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.EpisodeNumber).NotEmpty().GreaterThan(0);
        RuleFor(x => x.SeriesId).NotEmpty().GreaterThan(0);
        RuleFor(x => x.Video).MaxFileSize(5).AllowedExtensions(".m3u8");
    }
}

public record CreateEpisodeResult(long Id);

public class CreateEpisodeHandler(IUnitOfWork unitOfWork, ICloudStorage storage)
    : IRequestHandler<CreateEpisodeCommand, CreateEpisodeResult>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ICloudStorage _storage = storage;

    public async Task<CreateEpisodeResult> Handle(CreateEpisodeCommand request, CancellationToken cancellationToken)
    {
        var video = request.Video != null
            ? await _storage.UploadAsync("episode", request.Video) : null;

        var episode = Episode.Create(
            request.Name,
            request.EpisodeNumber,
            request.SeriesId,
            request.Overview,
            video?.ToString(),
            request.Runtime,
            request.AirDate
        );

        if (request.Actors != null)
        {
            var actors = request.Actors.Count != 0
                ? await _unitOfWork.Repository<Actor>()
                    .GetAllAsync(a => request.Actors.Contains(a.Id), cancellationToken: cancellationToken) : [];
            episode.AssignActors(actors);
        }

        await _unitOfWork.Repository<Episode>().AddAsync(episode, cancellationToken);
        await _unitOfWork.CommitAsync();
        return new CreateEpisodeResult(episode.Id);
    }
}
