using FluentValidation;

namespace Netflex.Application.UseCases.V1.User.Commands;

public record ChangePasswordCommand(string UserId, string SessionId,
    string OldPassword, string NewPassword)
    : ICommand;

public class ChangePasswordCommandValidator
    : AbstractValidator<ChangePasswordCommand>
{
    public ChangePasswordCommandValidator()
    {
        RuleFor(x => x.NewPassword).Length(6, 20);
    }
}

public class ChangePasswordHandler(IUnitOfWork unitOfWork)
    : ICommandHandler<ChangePasswordCommand>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    public async Task<Unit> Handle(ChangePasswordCommand request,
        CancellationToken cancellationToken)
    {
        var userRepository = _unitOfWork.Repository<Domain.Entities.User>();
        var user = await userRepository.GetAsync(u => u.Id == request.UserId, cancellationToken)
            ?? throw new UserNotFoundException();

        if (user.PasswordHash is null || !user.PasswordHash.Verify(request.OldPassword))
            throw new IncorrectPasswordException();

        user.ChangePassword(request.NewPassword, request.SessionId);

        await _unitOfWork.CommitAsync();
        return Unit.Value;
    }
}
