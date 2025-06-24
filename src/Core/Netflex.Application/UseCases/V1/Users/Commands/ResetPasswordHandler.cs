using FluentValidation;
using Netflex.Domain.ValueObjects;

namespace Netflex.Application.UseCases.V1.Users.Commands;

public record ResetPasswordCommand(string Email, string Otp, string NewPassword)
    : ICommand;

public class ResetPasswordCommandValidator
    : AbstractValidator<ResetPasswordCommand>
{
    public ResetPasswordCommandValidator()
    {
        RuleFor(x => x.Email).EmailAddress();
        RuleFor(x => x.Otp).Length(6);
        RuleFor(x => x.NewPassword).Length(6, 20);
    }
}

public class ResetPasswordHandler(IOtpGenerator otpGenerator, IUnitOfWork unitOfWork)
    : ICommandHandler<ResetPasswordCommand>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IOtpGenerator _otpGenerator = otpGenerator;
    public async Task<Unit> Handle(ResetPasswordCommand request,
        CancellationToken cancellationToken)
    {
        var isValid = await _otpGenerator.VerifyOtpAsync(request.Email, request.Otp, true, cancellationToken);
        if (!isValid) throw new InvalidOtpException();
        var user = await _unitOfWork.Repository<Domain.Entities.User>()
            .GetAsync(u => u.Email == Email.Of(request.Email), cancellationToken: cancellationToken)
            ?? throw new UserNotFoundException();

        user.ChangePassword(request.NewPassword);
        await _unitOfWork.CommitAsync();
        return Unit.Value;
    }
}
