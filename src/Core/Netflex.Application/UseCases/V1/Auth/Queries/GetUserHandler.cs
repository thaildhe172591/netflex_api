using System.Data;
using Netflex.Application.DTOs;
using Dapper;

namespace Netflex.Application.UseCases.V1.Auth.Queries;

public record GetUserQuery(string UserId) : IQuery<GetUserResult>;
public record GetUserResult(UserDTO User);

public class GetUserHandler(IDbConnection connection)
    : IQueryHandler<GetUserQuery, GetUserResult>
{
    private readonly IDbConnection _connection = connection;

    public async Task<GetUserResult> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        const string sql = @"
            SELECT 
                u.email, 
                COALESCE(string_agg(DISTINCT r.name, ','), '') AS roles,
                COALESCE(string_agg(DISTINCT p.name, ','), '') AS permissions
            FROM dbo.users u
            LEFT JOIN dbo.user_roles ur ON ur.users_id = u.user_id
            LEFT JOIN dbo.roles r ON r.role_id = ur.roles_id
            LEFT JOIN dbo.user_permissions up ON up.users_id = u.user_id
            LEFT JOIN dbo.permissions p ON p.permission_id = up.permissions_id
            WHERE u.user_id = @Id
            GROUP BY u.email";

        var result = await _connection.QueryFirstOrDefaultAsync<(string Email, string Roles, string Permissions)>(
            sql, new { Id = request.UserId });

        if (result == default) throw new UserNotFoundException();

        var roles = result.Roles.Split(',', StringSplitOptions.RemoveEmptyEntries);
        var permissions = result.Permissions.Split(',', StringSplitOptions.RemoveEmptyEntries);

        return new GetUserResult(new UserDTO(result.Email, roles, permissions));
    }
}