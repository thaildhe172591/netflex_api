using System.Linq.Expressions;
using Netflex.Application.Interfaces.Repositories;
using Netflex.Domain.Entities.Abstractions;

namespace Netflex.Persistence.Repositories;

public class BaseRepository<T>(ApplicationDbContext dbContext)
    : IBaseRepository<T> where T : class, IEntity
{
    protected readonly ApplicationDbContext _dbContext = dbContext;
    public virtual async Task AddAsync(T entity, CancellationToken cancellationToken = default)
        => await _dbContext.AddAsync(entity, cancellationToken);

    public virtual async Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
        => await _dbContext.AddRangeAsync(entities, cancellationToken);

    public virtual async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
        => await _dbContext.Set<T>().AsNoTracking().AnyAsync(predicate, cancellationToken);

    public T? Get(object id) => _dbContext.Set<T>().Find(id);

    public virtual async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>> predicate,
        string? includeProperties = default, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = default,
        CancellationToken cancellationToken = default)
    {
        IQueryable<T> query = _dbContext.Set<T>();
        if (predicate != null) query = query.Where(predicate);

        if (includeProperties != null)
        {
            foreach (var property in includeProperties.Split([','], StringSplitOptions.RemoveEmptyEntries))
                query = query.Include(property);
        }
        if (orderBy != null) query = orderBy(query);
        return await query.ToListAsync(cancellationToken);
    }

    public virtual async Task<T?> GetAsync(Expression<Func<T, bool>> predicate,
        string? includeProperties = default,
        CancellationToken cancellationToken = default)
    {
        IQueryable<T> query = _dbContext.Set<T>();
        if (predicate != null) query = query.Where(predicate);

        if (includeProperties != null)
        {
            foreach (var property in includeProperties.Split([','], StringSplitOptions.RemoveEmptyEntries))
                query = query.Include(property);
        }
        return await query.FirstOrDefaultAsync(cancellationToken);
    }

    public virtual void Remove(T entity)
        => _dbContext.Remove(entity);

    public virtual void RemoveRange(IEnumerable<T> entities)
        => _dbContext.RemoveRange(entities);

    public virtual void Update(T entity)
        => _dbContext.Update(entity);

    public virtual void UpdateRange(IEnumerable<T> entities)
        => _dbContext.UpdateRange(entities);
}