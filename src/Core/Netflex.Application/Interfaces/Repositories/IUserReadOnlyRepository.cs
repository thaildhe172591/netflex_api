using Netflex.Domain.Entities;

namespace Netflex.Application.Interfaces.Repositories;

public interface IUserReadOnlyRepository : IReadOnlyRepository<User>
{
    Task<UserDetailDTO?> GetUserDetailAsync(string userId);
}