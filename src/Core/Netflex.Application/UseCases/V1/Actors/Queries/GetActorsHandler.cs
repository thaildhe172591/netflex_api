using Netflex.Application.Interfaces.Repositories.ReadOnly;
using Netflex.Shared.Pagination;

namespace Netflex.Application.UseCases.V1.Actors.Queries;

public record GetActorsQuery(
    string? Search,
    string? SortBy,
    int PageIndex,
    int PageSize
) : IQuery<PaginatedResult<ActorDto>>;

public class GetActorsHandler(IActorReadOnlyRepository actorReadOnlyRepository)
    : IQueryHandler<GetActorsQuery, PaginatedResult<ActorDto>>
{
    private readonly IActorReadOnlyRepository _actorReadOnlyRepository = actorReadOnlyRepository;
    public async Task<PaginatedResult<ActorDto>> Handle(GetActorsQuery request, CancellationToken cancellationToken)
    {
        var result = await _actorReadOnlyRepository.GetActorsAsync(
            request.Search,
            request.SortBy,
            request.PageIndex,
            request.PageSize,
            cancellationToken
        );
        return result;
    }
}