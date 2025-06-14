using System.Linq.Expressions;
using Netflex.Application.Interfaces.Repositories;
using Netflex.Domain.Entities.Abstractions;

namespace Netflex.Persistence.Repositories;

public class BaseRepository<T>(ApplicationDbContext dbContext)
    : IBaseRepository<T> where T : class, IEntity
{
    protected readonly ApplicationDbContext _dbContext = dbContext;
    public async Task AddAsync(T entity, CancellationToken cancellationToken = default)
        => await _dbContext.AddAsync(entity, cancellationToken);

    public async Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
        => await _dbContext.AddRangeAsync(entities, cancellationToken);

    public async Task<bool> ExistsAsync(Expression<Func<T, bool>> expression, CancellationToken cancellationToken = default)
    {
        return await _dbContext.Set<T>().AsNoTracking().AnyAsync(expression, cancellationToken);
    }

    public async Task<IEnumerable<T>> GetAllAsync(Expression<Func<T, bool>>? expression, Func<IQueryable<T>, IQueryable<T>>? include = default,
        CancellationToken cancellationToken = default)
    {
        IQueryable<T> query = _dbContext.Set<T>();
        query = include is null ? query : include(query);
        query = expression is null ? query : query.Where(expression);
        return await query.ToListAsync(cancellationToken);
    }

    public async Task<T?> GetAsync(Expression<Func<T, bool>> expression, Func<IQueryable<T>, IQueryable<T>>? include = default,
        CancellationToken cancellationToken = default)
    {
        IQueryable<T> query = _dbContext.Set<T>();
        query = include is null ? query : include(query);
        return await query.FirstOrDefaultAsync(expression, cancellationToken);

    }

    public void Remove(T entity)
        => _dbContext.Remove(entity);


    public void RemoveRange(IEnumerable<T> entities)
        => _dbContext.RemoveRange(entities);


    public void Update(T entity)
        => _dbContext.Update(entity);


    public void UpdateRange(IEnumerable<T> entities)
        => _dbContext.UpdateRange(entities);
}