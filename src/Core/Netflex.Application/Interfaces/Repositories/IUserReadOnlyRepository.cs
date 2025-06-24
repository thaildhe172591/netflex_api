using Netflex.Application.Interfaces.Repositories.ReadOnly;

namespace Netflex.Application.Interfaces.Repositories;

public interface IUserReadOnlyRepository : IReadOnlyRepository
{
    Task<UserDetailDto?> GetUserDetailAsync(string userId);
}