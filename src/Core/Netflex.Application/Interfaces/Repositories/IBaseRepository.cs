using System.Linq.Expressions;
using Netflex.Domain.Entities.Abstractions;

namespace Netflex.Application.Interfaces.Repositories;

public interface IBaseRepository<T> where T : class, IEntity
{
    void Remove(T entity);
    void RemoveRange(IEnumerable<T> entities);
    void Update(T entity);
    void UpdateRange(IEnumerable<T> entities);
    Task<T?> GetAsync(Expression<Func<T, bool>> expression, Func<IQueryable<T>, IQueryable<T>>? include = default, CancellationToken cancellationToken = default);
    Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> expression, Func<IQueryable<T>, IQueryable<T>>? include = default, CancellationToken cancellationToken = default);
    Task AddAsync(T entity, CancellationToken cancellationToken = default);
    Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(Expression<Func<T, bool>> expression, CancellationToken cancellationToken = default);
}