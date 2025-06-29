using Netflex.Application.Interfaces.Repositories.ReadOnly;
using Netflex.Domain.Entities;
using Netflex.Shared.Exceptions;

namespace Netflex.Application.UseCases.V1.Episodes.Queries;

public record GetEpisodeQuery(long Id)
    : IQuery<EpisodeDetailDto>;

public class GetEpisodeHandler(IEpisodeReadOnlyRepository episodeReadOnlyRepository)
    : IQueryHandler<GetEpisodeQuery, EpisodeDetailDto>
{
    private readonly IEpisodeReadOnlyRepository _episodeReadOnlyRepository = episodeReadOnlyRepository;
    public async Task<EpisodeDetailDto> Handle(GetEpisodeQuery request, CancellationToken cancellationToken)
    {
        var result = await _episodeReadOnlyRepository.GetEpisodeAsync(
            request.Id,
            cancellationToken
        ) ?? throw new NotFoundException(nameof(Episode), request.Id);
        return result;
    }
}