using Netflex.Application.Interfaces.Repositories.ReadOnly;
using Netflex.Shared.Pagination;

namespace Netflex.Application.Interfaces.Repositories;

public interface IUserReadOnlyRepository : IReadOnlyRepository
{
    Task<UserDetailDto?> GetUserDetailAsync(string userId);

    Task<PaginatedResult<UserDto>> GetUsersAsync(
        string? search,
        string[]? roles,
        bool? isConfirmed,
        string? sortBy,
        int pageIndex,
        int pageSize,
        CancellationToken cancellationToken = default);
}