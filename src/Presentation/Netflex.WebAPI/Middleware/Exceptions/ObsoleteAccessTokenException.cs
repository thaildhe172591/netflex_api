using Netflex.Shared.Exceptions;

namespace Netflex.WebAPI.Middleware.Exceptions;

public class ObsoleteAccessTokenException : UnauthorizedException
{
    public ObsoleteAccessTokenException() : base("Access token is out of date")
    {
    }
}