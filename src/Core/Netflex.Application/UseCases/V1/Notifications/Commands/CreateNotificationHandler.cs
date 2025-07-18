using FluentValidation;
using Netflex.Domain.Entities;

namespace Netflex.Application.UseCases.V1.Notifications.Commands;

public record CreateNotificationCommand(string Title, string? Content, IEnumerable<string> UserId) : ICommand<CreateNotificationResult>;
public record CreateNotificationResult(long Id);

public class CreateNotificationCommandValidator : AbstractValidator<CreateNotificationCommand>
{
    public CreateNotificationCommandValidator()
    {
        RuleFor(x => x.Title).NotEmpty();
        RuleFor(x => x.UserId).NotEmpty();
    }
}

public class CreateNotificationHandler(IUnitOfWork unitOfWork)
    : ICommandHandler<CreateNotificationCommand, CreateNotificationResult>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<CreateNotificationResult> Handle(CreateNotificationCommand request, CancellationToken cancellationToken)
    {
        var notification = Notification.Create(request.Title, request.Content);
        var notificationRepository = _unitOfWork.Repository<Notification>();
        var userNotificationRepository = _unitOfWork.Repository<UserNotification>();

        await notificationRepository.AddAsync(notification, cancellationToken);
        await _unitOfWork.CommitAsync();

        foreach (var userId in request.UserId)
        {
            var userNotification = UserNotification.Create(userId, notification.Id);
            await userNotificationRepository.AddAsync(userNotification, cancellationToken);
        }

        await _unitOfWork.CommitAsync();

        return new CreateNotificationResult(notification.Id);
    }
}