using FluentValidation;
using Netflex.Domain.Entities;
using Netflex.Domain.ValueObjects;

namespace Netflex.Application.UseCases.V1.Reviews.Commands;

public record CreateOrUpdateReviewCommand(string UserId, string TargetId, string TargetType, int Rating)
    : ICommand<Unit>;

public class CreateOrUpdateReviewCommandValidator : AbstractValidator<CreateOrUpdateReviewCommand>
{
    public CreateOrUpdateReviewCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.TargetId).NotEmpty();
        RuleFor(x => x.TargetType).NotEmpty();
        RuleFor(x => x.Rating).NotEmpty();
    }
}

public class CreateOrUpdateReviewHandler(IUnitOfWork unitOfWork)
    : ICommandHandler<CreateOrUpdateReviewCommand>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Unit> Handle(CreateOrUpdateReviewCommand request, CancellationToken cancellationToken)
    {
        var repository = _unitOfWork.Repository<Review>();
        var target = TargetType.Of(request.TargetType);
        var review = await repository.GetAsync(
            r => r.UserId == request.UserId
            && r.TargetId == request.TargetId
            && r.TargetType == target, cancellationToken: cancellationToken);

        if (review is not null)
        {
            review.Update(Rating.Of(request.Rating));
            repository.Update(review);
        }
        else
        {
            review = Review.Create(request.TargetId, target, request.UserId, Rating.Of(request.Rating), null, DateTime.UtcNow);
            await repository.AddAsync(review, cancellationToken);
        }

        await _unitOfWork.CommitAsync();
        return Unit.Value;
    }
}