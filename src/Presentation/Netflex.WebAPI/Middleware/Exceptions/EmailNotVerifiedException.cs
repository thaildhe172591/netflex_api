using Netflex.Shared.Exceptions;

namespace Netflex.WebAPI.Middleware.Exceptions;

public class EmailNotVerifiedException(string email) : UnauthorizedException(email)
{
}