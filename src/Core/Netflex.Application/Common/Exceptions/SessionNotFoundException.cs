using Netflex.Shared.Exceptions;

namespace Netflex.Application.Common.Exceptions;

public class SessionNotFoundException()
    : NotFoundException("Session is not found");