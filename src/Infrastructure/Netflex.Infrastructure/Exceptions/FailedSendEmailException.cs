using Netflex.Shared.Exceptions;

namespace Netflex.Infrastructure.Exceptions;

public class FailedSendEmailException(string subject)
    : InternalServerException($"Failed to send email: {subject}")
{
}
