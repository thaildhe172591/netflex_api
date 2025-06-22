using Netflex.Shared.Pagination;

namespace Netflex.Application.UseCases.V1.Keywords.Queries;

public record GetKeywordsQuery(string? Search, string? SortBy, int PageIndex, int PageSize)
    : IQuery<PaginatedResult<KeywordDTO>>;

public class GetKeywordsHandler(IKeywordReadOnlyRepository keywordReadOnlyRepository)
    : IQueryHandler<GetKeywordsQuery, PaginatedResult<KeywordDTO>>
{
    private readonly IKeywordReadOnlyRepository _keywordReadOnlyRepository = keywordReadOnlyRepository;
    public async Task<PaginatedResult<KeywordDTO>> Handle(GetKeywordsQuery request, CancellationToken cancellationToken)
    {
        await _keywordReadOnlyRepository.GetKeywordsAsync(
            request.Search,
            request.SortBy,
            request.PageIndex,
            request.PageSize,
            cancellationToken
        );
        throw new NotImplementedException();
    }
}

