using Netflex.Domain.Entities;
using Netflex.Domain.ValueObjects;
using Netflex.Shared.Exceptions;

namespace Netflex.Application.UseCases.V1.Users.Commands;

public record GetFollowQuery(string UserId, string TargetId, string TargetType) : IQuery<GetFollowResult>;

public record GetFollowResult(FollowDto Follow);

public class GetFollowHandler(IUnitOfWork unitOfWork)
    : IQueryHandler<GetFollowQuery, GetFollowResult>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    public async Task<GetFollowResult> Handle(GetFollowQuery request, CancellationToken cancellationToken)
    {
        var follow = await _unitOfWork.Repository<Follow>().GetAsync(
            r => r.UserId == request.UserId && r.TargetId == request.TargetId && r.TargetType == TargetType.Of(request.TargetType),
            cancellationToken: cancellationToken
        ) ?? throw new NotFoundException("Follow not found");

        return new GetFollowResult(new FollowDto
        {
            TargetId = follow.TargetId,
            TargetType = follow.TargetType.Value,
            UserId = follow.UserId,
            CreatedAt = follow.CreatedAt
        });
    }
}
