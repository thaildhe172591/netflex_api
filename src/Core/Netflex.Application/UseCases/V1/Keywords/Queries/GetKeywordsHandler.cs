using Netflex.Application.Interfaces.Repositories.ReadOnly;
using Netflex.Shared.Pagination;

namespace Netflex.Application.UseCases.V1.Keywords.Queries;

public record GetKeywordsQuery(string? Search, string? SortBy, int PageIndex, int PageSize)
    : IQuery<PaginatedResult<KeywordDto>>;

public class GetKeywordsHandler(IKeywordReadOnlyRepository keywordReadOnlyRepository)
    : IQueryHandler<GetKeywordsQuery, PaginatedResult<KeywordDto>>
{
    private readonly IKeywordReadOnlyRepository _keywordReadOnlyRepository = keywordReadOnlyRepository;
    public async Task<PaginatedResult<KeywordDto>> Handle(GetKeywordsQuery request, CancellationToken cancellationToken)
    {
        var result = await _keywordReadOnlyRepository.GetKeywordsAsync(
            request.Search,
            request.SortBy,
            request.PageIndex,
            request.PageSize,
            cancellationToken
        );
        return result;
    }
}

