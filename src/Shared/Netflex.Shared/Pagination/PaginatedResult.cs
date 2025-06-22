namespace Netflex.Shared.Pagination;

public class PaginatedResult<TEntity>
    (IEnumerable<TEntity> data, long total, int pageIndex, int pageSize)
    where TEntity : class
{
    public int PageIndex { get; } = pageIndex;
    public int PageSize { get; } = pageSize;
    public long Total { get; } = total;
    public IEnumerable<TEntity> Data { get; } = data;
}
