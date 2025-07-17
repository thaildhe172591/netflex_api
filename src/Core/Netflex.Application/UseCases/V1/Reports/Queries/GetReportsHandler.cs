using Netflex.Application.Interfaces.Repositories.ReadOnly;
using Netflex.Shared.Pagination;

namespace Netflex.Application.UseCases.V1.Reports.Queries;


public record GetReportsQuery(string? Search, string? SortBy, int PageIndex, int PageSize)
    : IRequest<PaginatedResult<ReportDto>>;

public class GetReportsHandler : IRequestHandler<GetReportsQuery, PaginatedResult<ReportDto>>
{
    private readonly IReportReadOnlyRepository _reportReadOnlyRepository;

    public GetReportsHandler(IReportReadOnlyRepository reportReadOnlyRepository)
    {
        _reportReadOnlyRepository = reportReadOnlyRepository;
    }

    public async Task<PaginatedResult<ReportDto>> Handle(GetReportsQuery request, CancellationToken cancellationToken)
    {
        return await _reportReadOnlyRepository.GetReportsAsync(request.Search, request.SortBy,
            request.PageIndex, request.PageSize, cancellationToken);
    }
}