using Netflex.Shared.Exceptions;

namespace Netflex.Application.Exceptions;

public class SessionNotFoundException()
    : NotFoundException("Session is not found");