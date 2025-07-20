using Netflex.Shared.Pagination;

namespace Netflex.Application.UseCases.V1.Users.Queries;

public record GetUsersQuery(
    string? Search,
    string[]? Roles,
    bool? IsConfirmed,
    string? SortBy,
    int PageIndex,
    int PageSize
)
: IQuery<PaginatedResult<UserDto>>;

public class GetUsersHandler(IUserReadOnlyRepository userReadOnlyRepository)
    : IQueryHandler<GetUsersQuery, PaginatedResult<UserDto>>
{
    private readonly IUserReadOnlyRepository _userReadOnlyRepository = userReadOnlyRepository;

    public async Task<PaginatedResult<UserDto>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        var result = await _userReadOnlyRepository.GetUsersAsync(
            request.Search,
            request.Roles,
            request.IsConfirmed,
            request.SortBy,
            request.PageIndex,
            request.PageSize,
            cancellationToken
        );
        return result;
    }
}