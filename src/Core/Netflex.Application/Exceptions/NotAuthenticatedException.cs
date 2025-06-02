
using Netflex.Shared.Exceptions;

namespace Netflex.Application.Exceptions;

public class NotAuthenticatedException()
    : UnauthorizedException("User is not authenticated")
{

}

