using FluentValidation;
using Netflex.Domain.ValueObjects;

namespace Netflex.Application.UseCases.V1.User.Commands;

public record ResetPasswordCommand(string Email, string OTP, string NewPassword)
    : ICommand;

public class ResetPasswordCommandValidator
    : AbstractValidator<ResetPasswordCommand>
{
    public ResetPasswordCommandValidator()
    {
        RuleFor(x => x.Email).EmailAddress();
        RuleFor(x => x.OTP).Length(6);
        RuleFor(x => x.NewPassword).Length(6, 20);
    }
}

public class ResetPasswordHandler(IOTPGenerator otpGenerator, IUnitOfWork unitOfWork)
    : ICommandHandler<ResetPasswordCommand>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IOTPGenerator _otpGenerator = otpGenerator;
    public async Task<Unit> Handle(ResetPasswordCommand request,
        CancellationToken cancellationToken)
    {
        var isValid = await _otpGenerator.VerifyOTPAsync(request.Email, request.OTP, true, cancellationToken);
        if (!isValid) throw new InvalidOTPException();
        var user = await _unitOfWork.Repository<Domain.Entities.User>()
            .GetAsync(u => u.Email == Email.Of(request.Email), cancellationToken: cancellationToken)
            ?? throw new UserNotFoundException();

        user.ChangePassword(request.NewPassword);
        await _unitOfWork.CommitAsync();
        return Unit.Value;
    }
}
