using FluentValidation;

namespace Netflex.Application.UseCases.V1.Auth.Commands;

public record SendOTPCommand(string Email) : ICommand;

public class SendOTPCommandValidator
    : AbstractValidator<SendOTPCommand>
{
    public SendOTPCommandValidator()
    {
        RuleFor(x => x.Email).EmailAddress();
    }
}

public class SendOTPHandler(IEmailService emailService,
    ITemplateService templateService, IOTPGenerator otpGenerator)
    : ICommandHandler<SendOTPCommand>
{
    private readonly IOTPGenerator _otpGenerator = otpGenerator;
    private readonly IEmailService _emailService = emailService;
    private readonly ITemplateService _templateService = templateService;
    public async Task<Unit> Handle(SendOTPCommand request, CancellationToken cancellationToken)
    {
        var otp = await _otpGenerator.GenerateOTPAsync(request.Email, cancellationToken);
        var company = _emailService.Settings.Company;
        var html = _templateService.GenerateOTPEmail(otp, company);
        await _emailService.SendEmailAsync(request.Email, $"{company} OTP Verification", html, cancellationToken);
        return Unit.Value;
    }
}