using Netflex.Application.DTOs;
using Netflex.Shared.Pagination;

namespace Netflex.Application.Interfaces.Repositories.ReadOnly;

public interface IReportReadOnlyRepository : IReadOnlyRepository
{
    public Task<PaginatedResult<ReportDto>> GetReportsAsync(string? search, string? sortBy,
        int pageIndex, int pageSize, CancellationToken cancellationToken = default);
}