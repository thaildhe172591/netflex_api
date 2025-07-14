using Netflex.Shared.Pagination;

namespace Netflex.Application.Interfaces.Repositories.ReadOnly;

public interface IActorReadOnlyRepository : IReadOnlyRepository
{
    Task<PaginatedResult<ActorDto>> GetActorsAsync(
        string? search, string? sortBy, int pageIndex, int pageSize, CancellationToken cancellationToken = default);
    Task<ActorDto?> GetByIdAsync(long id, CancellationToken cancellationToken = default);
}
