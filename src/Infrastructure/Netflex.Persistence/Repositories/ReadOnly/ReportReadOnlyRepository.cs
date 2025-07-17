using System.Data;
using System.Text;
using Dapper;
using Netflex.Application.DTOs;
using Netflex.Application.Interfaces.Repositories.ReadOnly;
using Netflex.Shared.Pagination;

namespace Netflex.Persistence.Repositories.ReadOnly;

public class ReportReadOnlyRepository : ReadOnlyRepository, IReportReadOnlyRepository
{
    public ReportReadOnlyRepository(IDbConnection connection) : base(connection)
    {
        _columns = ["report_id", "reason", "description", "process"];
    }

    public Task<PaginatedResult<ReportDto>> GetReportsAsync(string? search, string? sortBy,
        int pageIndex, int pageSize, CancellationToken cancellationToken = default)
    {
        var query = new StringBuilder(@"
            SELECT  report_id as id, reason, description, process
            FROM dbo.reports
            WHERE 1 = 1
        ");
        var parameters = new DynamicParameters();

        if (!string.IsNullOrWhiteSpace(search))
        {
            query.AppendLine("AND (reason ILIKE @Search OR description ILIKE @Search)");
            parameters.Add("Search", $"%{search}%");
        }

        return GetPagedDataAsync<ReportDto>(query.ToString(), sortBy, pageIndex, pageSize, parameters);
    }
}