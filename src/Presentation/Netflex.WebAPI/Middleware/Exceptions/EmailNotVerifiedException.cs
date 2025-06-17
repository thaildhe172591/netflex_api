using Netflex.Shared.Exceptions;

namespace Netflex.WebAPI.Middleware.Exceptions;

public class EmailNotVerifiedException(string email) : ForbiddenException(email)
{
}