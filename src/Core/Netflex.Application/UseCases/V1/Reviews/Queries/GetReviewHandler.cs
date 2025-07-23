using Netflex.Domain.Entities;
using Netflex.Domain.ValueObjects;
using Netflex.Shared.Exceptions;

namespace Netflex.Application.UseCases.V1.Reviews.Queries;

public record GetReviewQuery(string UserId, string TargetId, string TargetType) : IQuery<GetReviewResult>;

public record GetReviewResult(ReviewDto Review);

public class GetReviewHandler(IUnitOfWork unitOfWork)
    : IQueryHandler<GetReviewQuery, GetReviewResult>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    public async Task<GetReviewResult> Handle(GetReviewQuery request, CancellationToken cancellationToken)
    {
        var review = await _unitOfWork.Repository<Review>().GetAsync(
            r => r.UserId == request.UserId && r.TargetId == request.TargetId && r.TargetType == TargetType.Of(request.TargetType),
            cancellationToken: cancellationToken
        ) ?? throw new NotFoundException("Review not found");

        return new GetReviewResult(new ReviewDto
        {
            TargetId = review.TargetId,
            TargetType = review.TargetType.Value,
            UserId = review.UserId,
            Rating = review.Rating.Value,
            Comment = review.Comment,
            LikeCount = review.LikeCount,
            CreatedAt = review.CreatedAt
        });
    }
}