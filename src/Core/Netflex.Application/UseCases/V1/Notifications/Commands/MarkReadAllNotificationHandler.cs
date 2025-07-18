using FluentValidation;
using Netflex.Domain.Entities;

namespace Netflex.Application.UseCases.V1.Notifications.Commands;

public record MarkReadAllNotificationCommand(string UserId) : ICommand;

public class MarkReadAllNotificationCommandValidator : AbstractValidator<MarkReadAllNotificationCommand>
{
    public MarkReadAllNotificationCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty();
    }
}

public class MarkReadAllNotificationHandler(IUnitOfWork unitOfWork)
    : ICommandHandler<MarkReadAllNotificationCommand>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Unit> Handle(MarkReadAllNotificationCommand request, CancellationToken cancellationToken)
    {
        var userNotificationRepository = _unitOfWork.Repository<UserNotification>();

        var userNotifications = await userNotificationRepository.GetAllAsync(
            x => x.UserId == request.UserId && !x.HaveRead,
            cancellationToken: cancellationToken);

        if (userNotifications.Any())
        {
            foreach (var userNotification in userNotifications)
            {
                userNotification.MarkAsRead();
            }
            userNotificationRepository.UpdateRange(userNotifications);
            await _unitOfWork.CommitAsync();
        }

        return Unit.Value;
    }
}