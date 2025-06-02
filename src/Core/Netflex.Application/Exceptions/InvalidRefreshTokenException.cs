using Netflex.Shared.Exceptions;

namespace Netflex.Application.Exceptions;

public class InvalidRefreshTokenException : BadRequestException
{
    public InvalidRefreshTokenException() : base("Refresh token is not valid") { }
}