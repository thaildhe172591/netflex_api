using Netflex.Shared.Exceptions;

namespace Netflex.Application.Exceptions;

public class InvalidOTPException : BadRequestException
{
    public InvalidOTPException() : base("OTP is not valid") { }
}