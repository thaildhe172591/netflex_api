using System.Data;
using System.Text;
using Dapper;
using Netflex.Application.DTOs;
using Netflex.Application.Interfaces.Repositories;
using Netflex.Shared.Pagination;

namespace Netflex.Persistence.Repositories.ReadOnly;

public class UserReadOnlyRepository(IDbConnection connection)
    : ReadOnlyRepository(connection), IUserReadOnlyRepository
{
    public async Task<UserDetailDto?> GetUserDetailAsync(string userId)
    {
        const string sql = @"
            SELECT 
                u.email, 
                u.email_confirmed as confirmed,
                COALESCE(string_agg(DISTINCT r.name, ','), '') AS roles,
                COALESCE(string_agg(DISTINCT p.name, ','), '') AS permissions
            FROM dbo.users u
            LEFT JOIN dbo.user_roles ur ON ur.user_id = u.user_id
            LEFT JOIN dbo.roles r ON r.role_id = ur.role_id
            LEFT JOIN dbo.user_permissions up ON up.user_id = u.user_id
            LEFT JOIN dbo.permissions p ON p.permission_id = up.permission_id
            WHERE u.user_id = @Id
            GROUP BY u.email, u.email_confirmed";

        var result = await _connection.QueryFirstOrDefaultAsync<(string Email, bool Confirmed, string Roles, string Permissions)>(
            sql, new { Id = userId });

        if (result == default) return null;

        var roles = result.Roles.Split(',', StringSplitOptions.RemoveEmptyEntries);
        var permissions = result.Permissions.Split(',', StringSplitOptions.RemoveEmptyEntries);
        return new UserDetailDto(result.Email, result.Confirmed, roles, permissions);
    }

    public Task<PaginatedResult<UserDto>> GetUsersAsync(
        string? search,
        string[]? roles,
        bool? isConfirmed,
        string? sortBy,
        int pageIndex,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        var query = new StringBuilder(@"
            WITH UsersCTE AS (
                SELECT 
                    u.user_id AS id,
                    u.email, 
                    u.email_confirmed as confirmed,
                    COALESCE(string_agg(DISTINCT r.name, ','), '') AS roles,
                    COALESCE(string_agg(DISTINCT p.name, ','), '') AS permissions
                FROM dbo.users u
                LEFT JOIN dbo.user_roles ur ON ur.user_id = u.user_id
                LEFT JOIN dbo.roles r ON r.role_id = ur.role_id
                LEFT JOIN dbo.user_permissions up ON up.user_id = u.user_id
                LEFT JOIN dbo.permissions p ON p.permission_id = up.permission_id
                GROUP BY u.user_id, u.email, u.email_confirmed
            )
            SELECT *
            FROM UsersCTE
            WHERE 1=1");

        var parameters = new DynamicParameters();

        if (!string.IsNullOrWhiteSpace(search))
        {
            query.Append(" AND email ILIKE @Search");
            parameters.Add("Search", $"%{search}%");
        }

        if (roles?.Length > 0)
        {
            query.Append(" AND roles && @Roles");
            parameters.Add("Roles", roles);
        }

        if (isConfirmed.HasValue)
        {
            query.Append(" AND confirmed = @IsConfirmed");
            parameters.Add("IsConfirmed", isConfirmed.Value);
        }

        return GetPagedDataAsync<UserDto>(
             query.ToString(),
             sortBy,
             pageIndex,
             pageSize,
             parameters

         );

    }



}