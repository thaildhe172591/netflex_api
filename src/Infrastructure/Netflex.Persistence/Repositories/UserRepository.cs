using Netflex.Application.Interfaces.Repositories;
using Netflex.Application.Exceptions;

namespace Netflex.Persistence.Repositories;

public class UserRepository(ApplicationDbContext dbContext)
    : BaseRepository<User>(dbContext), IUserRepository
{
    public async Task<int> GetVersionByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        var user = await _dbContext.Users.AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == id, cancellationToken)
            ?? throw new UserNotFoundException();
        return user.Version;
    }

    public async Task ResetVersionByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Id == id, cancellationToken)
            ?? throw new UserNotFoundException();
        user.Version++;
        _dbContext.Update(user);
    }
}