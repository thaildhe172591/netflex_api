using Netflex.Shared.Pagination;

namespace Netflex.Application.Interfaces.Repositories.ReadOnly;

public interface INotificationReadOnlyRepository : IReadOnlyRepository
{
    public Task<PaginatedResult<NotificationDto>> GetNotificationsAsync(string? search, string? UserId, string? sortBy,
        int pageIndex, int pageSize, CancellationToken cancellationToken = default);
}