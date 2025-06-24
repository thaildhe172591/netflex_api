using FluentValidation;
using Netflex.Domain.ValueObjects;

namespace Netflex.Application.UseCases.V1.Auth.Commands;

public record ConfirmEmailCommand(string Email, string Otp) : ICommand;

public class ConfirmEmailCommandValidator
    : AbstractValidator<ConfirmEmailCommand>
{
    public ConfirmEmailCommandValidator()
    {
        RuleFor(x => x.Email).EmailAddress();
        RuleFor(x => x.Otp).Length(6);
    }
}

public class ConfirmEmailHandler(IOtpGenerator otpGenerator, IUnitOfWork unitOfWork)
    : ICommandHandler<ConfirmEmailCommand>
{
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IOtpGenerator _otpGenerator = otpGenerator;
    public async Task<Unit> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
    {
        var isValid = await _otpGenerator.VerifyOtpAsync(request.Email, request.Otp, true, cancellationToken);
        if (!isValid) throw new InvalidOtpException();

        var user = await _unitOfWork.Repository<Domain.Entities.User>()
            .GetAsync(u => u.Email == Email.Of(request.Email), cancellationToken: cancellationToken)
            ?? throw new UserNotFoundException();

        user.EmailConfirmed = true;

        await _unitOfWork.CommitAsync();
        return Unit.Value;
    }
}