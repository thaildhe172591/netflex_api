namespace Netflex.Application.Interfaces.Repositories;

public interface IUserReadOnlyRepository : IReadOnlyRepository
{
    Task<UserDetailDTO?> GetUserDetailAsync(string userId);
}