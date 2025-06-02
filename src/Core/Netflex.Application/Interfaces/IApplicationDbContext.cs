using Netflex.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Netflex.Application.Interfaces;

public interface IApplicationDbContext
{
    DbSet<User> Users { get; }
    DbSet<UserSession> UserTokens { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}