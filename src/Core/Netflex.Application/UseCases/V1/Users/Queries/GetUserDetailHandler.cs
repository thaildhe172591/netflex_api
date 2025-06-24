namespace Netflex.Application.UseCases.V1.Users.Queries;

public record GetUserDetailQuery(string UserId) : IQuery<GetUserDetailResult>;
public record GetUserDetailResult(UserDetailDto User);

public class GetUserDetailHandler(IUserReadOnlyRepository userReadOnlyRepository)
    : IQueryHandler<GetUserDetailQuery, GetUserDetailResult>
{
    private readonly IUserReadOnlyRepository _userReadOnlyRepository = userReadOnlyRepository;
    public async Task<GetUserDetailResult> Handle(GetUserDetailQuery request, CancellationToken cancellationToken)
    {
        var userDetail = await _userReadOnlyRepository.GetUserDetailAsync(request.UserId)
            ?? throw new UserNotFoundException();

        return new GetUserDetailResult(userDetail);
    }
}