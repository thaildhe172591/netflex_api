using System.Data;
using System.Text;
using Dapper;
using Netflex.Application.DTOs;
using Netflex.Application.Interfaces.Repositories.ReadOnly;
using Netflex.Shared.Pagination;

namespace Netflex.Persistence.Repositories.ReadOnly;

public class NotificationReadOnlyRepository : ReadOnlyRepository, INotificationReadOnlyRepository
{
    public NotificationReadOnlyRepository(IDbConnection connection) : base(connection)
    {
        _columns = ["notification_id", "title", "content", "created_at"];
    }

    public Task<PaginatedResult<NotificationDto>> GetNotificationsAsync(string? search, string? userId, string? sortBy,
        int pageIndex, int pageSize, CancellationToken cancellationToken = default)
    {
        var query = new StringBuilder(@"
            SELECT n.notification_id as id, n.title, n.content, n.created_at as createdAt
            FROM dbo.notifications n
            WHERE 1 = 1
        ");
        var parameters = new DynamicParameters();

        if (!string.IsNullOrWhiteSpace(search))
        {
            query.AppendLine("AND (n.title ILIKE @Search OR n.content ILIKE @Search)");
            parameters.Add("Search", $"%{search}%");
        }

        if (!string.IsNullOrWhiteSpace(userId))
        {
            query.AppendLine(@"
                AND EXISTS (
                    SELECT 1 FROM dbo.user_notifications un
                    WHERE un.notification_id = n.id
                    AND un.user_id = @UserId
                )
            ");
            parameters.Add("UserId", userId);
        }

        return GetPagedDataAsync<NotificationDto>(query.ToString(), sortBy, pageIndex, pageSize, parameters);
    }
}