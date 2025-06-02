using Netflex.Domain.Entities.Abstractions;

namespace Netflex.Application.Interfaces.Repositories;

public interface IUnitOfWork
{
    IBaseRepository<T> Repository<T>() where T : class, IEntity;
    void Commit();
    void Rollback();
    Task CommitAsync();
    Task RollbackAsync();
}