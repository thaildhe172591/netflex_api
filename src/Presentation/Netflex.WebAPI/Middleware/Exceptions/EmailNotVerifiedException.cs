using Netflex.Shared.Exceptions;

namespace Netflex.WebAPI.Middleware.Exceptions;

public class EmailNotVerifiedException : UnauthorizedException
{
    public EmailNotVerifiedException() : base("Email not verified") { }
}