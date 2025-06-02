using FluentValidation;
using Netflex.Domain.ValueObjects;

namespace Netflex.Application.UseCases.V1.Auth.Commands;

public record ConfirmEmailCommand(string Email, string OTP) : ICommand;

public class ConfirmEmailCommandValidator
    : AbstractValidator<ConfirmEmailCommand>
{
    public ConfirmEmailCommandValidator()
    {
        RuleFor(x => x.Email).EmailAddress();
        RuleFor(x => x.OTP).Length(6);
    }
}

public class ConfirmEmailHandler(IOTPGenerator otpGenerator, IUnitOfWork unitOfWork)
    : ICommandHandler<ConfirmEmailCommand>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IOTPGenerator _otpGenerator = otpGenerator;
    public async Task<Unit> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
    {
        var isValid = await _otpGenerator.VerifyOTPAsync(request.Email, request.OTP, true, cancellationToken);
        if (!isValid) throw new InvalidOTPException();

        var user = await _unitOfWork.Repository<Domain.Entities.User>()
            .GetAsync(u => u.Email == Email.Of(request.Email), cancellationToken: cancellationToken)
            ?? throw new UserNotFoundException();

        user.EmailConfirmed = true;

        await _unitOfWork.CommitAsync();
        return Unit.Value;
    }
}