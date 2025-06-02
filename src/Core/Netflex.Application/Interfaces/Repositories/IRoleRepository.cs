using Netflex.Domain.Entities;
namespace Netflex.Application.Interfaces.Repositories;

public interface IRoleRepository : IBaseRepository<User>
{
    Task<string> GetRoleNamesByUserIdAsync(string id, CancellationToken cancellationToken = default);
}