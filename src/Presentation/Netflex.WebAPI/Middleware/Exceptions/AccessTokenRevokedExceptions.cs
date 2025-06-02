using Netflex.Shared.Exceptions;

namespace Netflex.WebAPI.Middleware.Exceptions;

public class AccessTokenRevokedExceptions : UnauthorizedException
{
    public AccessTokenRevokedExceptions() : base("Access token is revoked")
    {
    }
}