namespace Netflex.Application.Interfaces;

public interface ITemplateService
{
    string GenerateOTPEmail(string otp, string company);
}