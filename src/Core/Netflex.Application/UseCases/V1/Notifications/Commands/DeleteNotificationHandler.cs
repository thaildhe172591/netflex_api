using FluentValidation;
using Netflex.Domain.Entities;
using Netflex.Shared.Exceptions;

namespace Netflex.Application.UseCases.V1.Notifications.Commands;

public record DeleteNotificationCommand(long Id) : ICommand<Unit>;

public class DeleteNotificationCommandValidator : AbstractValidator<DeleteNotificationCommand>
{
    public DeleteNotificationCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}

public class DeleteNotificationHandler(IUnitOfWork unitOfWork)
    : ICommandHandler<DeleteNotificationCommand>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Unit> Handle(DeleteNotificationCommand request, CancellationToken cancellationToken)
    {
        var notificationRepository = _unitOfWork.Repository<Notification>();
        var userNotificationRepository = _unitOfWork.Repository<UserNotification>();

        var notification = await notificationRepository.GetAsync(x => x.Id == request.Id, cancellationToken: cancellationToken)
            ?? throw new NotFoundException(nameof(Notification), request.Id);

        var userNotifications = await userNotificationRepository
            .GetAllAsync(x => x.NotificationId == request.Id, cancellationToken: cancellationToken);

        notificationRepository.Remove(notification);
        userNotificationRepository.RemoveRange(userNotifications);

        await _unitOfWork.CommitAsync();
        return Unit.Value;
    }
}