using Netflex.Domain.Entities;
namespace Netflex.Application.Interfaces.Repositories;

public interface IUserRepository : IBaseRepository<User>
{
    Task<int> GetVersionByIdAsync(string id, CancellationToken cancellationToken = default);
    Task ResetVersionByIdAsync(string id, CancellationToken cancellationToken = default);
}