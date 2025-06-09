using Netflex.Shared.Exceptions;

namespace Netflex.Infrastructure.Exceptions;

public class EmailNotSentException(string subject)
    : InternalServerException($"Failed to send email: {subject}")
{
}
