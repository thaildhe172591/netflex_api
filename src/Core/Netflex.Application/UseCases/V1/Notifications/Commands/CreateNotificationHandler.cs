using FluentValidation;

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
        await Task.CompletedTask;
        return new CreateNotificationResult(1);
    }
}