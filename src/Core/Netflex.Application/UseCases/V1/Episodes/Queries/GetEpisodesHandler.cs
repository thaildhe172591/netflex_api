using Netflex.Application.Interfaces.Repositories.ReadOnly;
using Netflex.Shared.Pagination;

namespace Netflex.Application.UseCases.V1.Episodes.Queries;

public record GetEpisodesQuery(
    string? Search,
    long? SeriesId,
    IEnumerable<long>? ActorIds,
    string? SortBy,
    int PageIndex,
    int PageSize
) : IQuery<PaginatedResult<EpisodeDto>>;

public class GetEpisodesHandler(IEpisodeReadOnlyRepository episodeReadOnlyRepository)
    : IQueryHandler<GetEpisodesQuery, PaginatedResult<EpisodeDto>>
{
    private readonly IEpisodeReadOnlyRepository _episodeReadOnlyRepository = episodeReadOnlyRepository;
    public async Task<PaginatedResult<EpisodeDto>> Handle(GetEpisodesQuery request, CancellationToken cancellationToken)
    {
        var result = await _episodeReadOnlyRepository.GetEpisodesAsync(
            request.Search,
            request.SeriesId,
            request.ActorIds,
            request.SortBy,
            request.PageIndex,
            request.PageSize,
            cancellationToken
        );
        return result;
    }
}