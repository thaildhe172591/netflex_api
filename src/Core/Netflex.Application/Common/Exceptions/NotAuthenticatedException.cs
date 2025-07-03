using Netflex.Shared.Exceptions;

namespace Netflex.Application.Common.Exceptions;

public class NotAuthenticatedException()
    : UnauthorizedException("User is not authenticated")
{

}

