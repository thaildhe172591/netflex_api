using Netflex.Shared.Exceptions;

namespace Netflex.Application.Exceptions;

public class UserNotFoundException()
    : NotFoundException("User is not found");