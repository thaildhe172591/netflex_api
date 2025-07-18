using FluentValidation;
using Netflex.Domain.Entities;
using Netflex.Shared.Exceptions;

namespace Netflex.Application.UseCases.V1.Notifications.Commands;

public record MarkReadNotificationCommand(long NotificationId, string UserId) : ICommand;

public class MarkReadNotificationCommandValidator : AbstractValidator<MarkReadNotificationCommand>
{
    public MarkReadNotificationCommandValidator()
    {
        RuleFor(x => x.NotificationId).NotEmpty();
        RuleFor(x => x.UserId).NotEmpty();
    }
}

public class MarkReadNotificationHandler(IUnitOfWork unitOfWork)
    : ICommandHandler<MarkReadNotificationCommand>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Unit> Handle(MarkReadNotificationCommand request, CancellationToken cancellationToken)
    {
        var userNotificationRepository = _unitOfWork.Repository<UserNotification>();

        var userNotification = await userNotificationRepository.GetAsync(
            x => x.NotificationId == request.NotificationId && x.UserId == request.UserId,
            cancellationToken: cancellationToken)
            ?? throw new NotFoundException($"User notification for NotificationId '{request.NotificationId}' and UserId '{request.UserId}' not found.");

        userNotification.MarkAsRead();
        userNotificationRepository.Update(userNotification);

        await _unitOfWork.CommitAsync();

        return Unit.Value;
    }
}