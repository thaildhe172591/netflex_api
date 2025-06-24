using System.Data;
using Dapper;
using Netflex.Application.Dtos;
using Netflex.Application.Interfaces.Repositories;
namespace Netflex.Persistence.Repositories.ReadOnly;

public class UserReadOnlyRepository(IDbConnection connection)
    : ReadOnlyRepository(connection), IUserReadOnlyRepository
{
    public async Task<UserDetailDto?> GetUserDetailAsync(string userId)
    {
        const string sql = @"
            SELECT 
                u.email, 
                COALESCE(string_agg(DISTINCT r.name, ','), '') AS roles,
                COALESCE(string_agg(DISTINCT p.name, ','), '') AS permissions
            FROM dbo.users u
            LEFT JOIN dbo.user_roles ur ON ur.user_id = u.user_id
            LEFT JOIN dbo.roles r ON r.role_id = ur.role_id
            LEFT JOIN dbo.user_permissions up ON up.user_id = u.user_id
            LEFT JOIN dbo.permissions p ON p.permission_id = up.permission_id
            WHERE u.user_id = @Id
            GROUP BY u.email";

        var result = await _connection.QueryFirstOrDefaultAsync<(string Email, string Roles, string Permissions)>(
            sql, new { Id = userId });

        if (result == default) return null;

        var roles = result.Roles.Split(',', StringSplitOptions.RemoveEmptyEntries);
        var permissions = result.Permissions.Split(',', StringSplitOptions.RemoveEmptyEntries);
        return new UserDetailDto(result.Email, roles, permissions);
    }
}