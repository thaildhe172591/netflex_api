using Netflex.Domain.Entities;
using FluentValidation;
using Netflex.Shared.Exceptions;
using Netflex.Application.Extensions;

namespace Netflex.Application.UseCases.V1.Episodes.Commands;

public record UpdateEpisodeCommand(
    long Id,
    string? Name,
    int? EpisodeNumber,
    long? SeriesId,
    string? Overview,
    IFileResource? Video,
    int? Runtime,
    DateOnly? AirDate,
    ICollection<long>? Actors
) : IRequest<UpdateEpisodeResult>;


public class UpdateEpisodeCommandValidator : AbstractValidator<UpdateEpisodeCommand>
{
    public UpdateEpisodeCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().GreaterThan(0);
        RuleFor(x => x.Name).NotEmpty().Unless(x => x.Name == null);
        RuleFor(x => x.EpisodeNumber).NotEmpty().GreaterThan(0).Unless(x => x.EpisodeNumber == null);
        RuleFor(x => x.SeriesId).NotEmpty().GreaterThan(0).Unless(x => x.SeriesId == null);
        RuleFor(x => x.Video).MaxFileSize(5).AllowedExtensions(".m3u8");
    }
}

public record UpdateEpisodeResult(long Id);

public class UpdateEpisodeHandler(IUnitOfWork unitOfWork, ICloudStorage storage)
    : IRequestHandler<UpdateEpisodeCommand, UpdateEpisodeResult>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ICloudStorage _storage = storage;

    public async Task<UpdateEpisodeResult> Handle(UpdateEpisodeCommand request, CancellationToken cancellationToken)
    {
        var episodeRepository = _unitOfWork.Repository<Episode>();
        var episode = await episodeRepository.GetAsync(x => x.Id == request.Id,
            includeProperties: [nameof(Episode.Actors)],
            cancellationToken: cancellationToken)
                ?? throw new NotFoundException(nameof(Episode), request.Id);
        var video = request.Video != null
            ? await _storage.UploadAsync("episode", request.Video) : null;

        episode.Update(
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

        episodeRepository.Update(episode);
        await _unitOfWork.CommitAsync();

        return new UpdateEpisodeResult(episode.Id);
    }
}