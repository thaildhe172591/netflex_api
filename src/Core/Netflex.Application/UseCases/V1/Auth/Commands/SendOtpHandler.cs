using FluentValidation;
using Netflex.Domain.ValueObjects;

namespace Netflex.Application.UseCases.V1.Auth.Commands;

public record SendOtpCommand(string Email) : ICommand;

public class SendOtpCommandValidator
    : AbstractValidator<SendOtpCommand>
{
    public SendOtpCommandValidator()
    {
        RuleFor(x => x.Email).EmailAddress();
    }
}

public class SendOtpHandler(IEmailService emailService, IOtpGenerator otpGenerator, IUnitOfWork unitOfWork)
    : ICommandHandler<SendOtpCommand>
{
    private readonly IOtpGenerator _otpGenerator = otpGenerator;
    private readonly IEmailService _emailService = emailService;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    public async Task<Unit> Handle(SendOtpCommand request, CancellationToken cancellationToken)
    {
        var hasExists = await _unitOfWork.Repository<Domain.Entities.User>()
            .ExistsAsync(u => u.Email == Email.Of(request.Email), cancellationToken);
        if (!hasExists) throw new UserNotFoundException();
        var otp = await _otpGenerator.GenerateOtpAsync(request.Email, cancellationToken);
        var company = _emailService.Settings.Company;
        var html = GetOtpEmail(otp, company);
        await _emailService.SendEmailAsync(request.Email, $"{company} Otp Verification", html, cancellationToken);
        return Unit.Value;
    }


    private static string GetOtpEmail(string otp, string company)
    {
        return $"""
        <div style="font-family: Arial, sans-serif; max-width: 500px; margin: auto; padding: 20px; border: 1px solid #e0e0e0; border-radius: 8px; background-color: #f9f9f9;">
            <h2 style="color: #333;">🔐 Your One-Time Password</h2>
            <p style="font-size: 16px; color: #555;">Use the following OTP to verify your email address:</p>
    
            <div style="font-size: 32px; font-weight: bold; color: #2c3e50; padding: 15px; background-color: #ffffff; border: 1px dashed #ccc; text-align: center; letter-spacing: 4px; border-radius: 6px;">
                {otp}
            </div>
    
            <p style="font-size: 14px; color: #999; margin-top: 20px;">
                This code will expire in 5 minutes. If you did not request this code, please ignore this email.
            </p>
    
            <p style="font-size: 14px; color: #777; margin-top: 30px;">Thanks,<br/>{company}</p>
        </div>
        """;
    }
}