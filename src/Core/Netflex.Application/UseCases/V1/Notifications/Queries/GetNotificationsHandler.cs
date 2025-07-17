using Netflex.Application.Interfaces.Repositories.ReadOnly;
using Netflex.Shared.Pagination;

namespace Netflex.Application.UseCases.V1.Notifications.Queries;

public record GetNotificationsQuery(string? Search, string? UserId, string? SortBy, int PageIndex, int PageSize)
    : IQuery<PaginatedResult<NotificationDto>>;

public class GetNotificationsHandler(INotificationReadOnlyRepository notificationReadOnlyRepository)
    : IQueryHandler<GetNotificationsQuery, PaginatedResult<NotificationDto>>
{
    private readonly INotificationReadOnlyRepository _notificationReadOnlyRepository = notificationReadOnlyRepository;
    public async Task<PaginatedResult<NotificationDto>> Handle(GetNotificationsQuery request, CancellationToken cancellationToken)
    {
        var result = await _notificationReadOnlyRepository.GetNotificationsAsync(
            request.Search,
            request.UserId,
            request.SortBy,
            request.PageIndex,
            request.PageSize,
            cancellationToken
        );
        return result;
    }
}

