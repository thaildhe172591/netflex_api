using Netflex.Shared.Pagination;

namespace Netflex.Application.UseCases.V1.Keyword.Queries;

public record GetKeywordsQuery(string? Query, string? SortBy, int PageIndex = 1, int PageSize = 10)
    : IQuery<PaginatedResult<KeywordDTO>>;
public class GetKeywordsHandler : IQueryHandler<GetKeywordsQuery, PaginatedResult<KeywordDTO>>
{
    public Task<PaginatedResult<KeywordDTO>> Handle(GetKeywordsQuery request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}

