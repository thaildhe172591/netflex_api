using Netflex.Shared.Exceptions;

namespace Netflex.Application.Common.Exceptions;

public class UserNotFoundException()
    : NotFoundException("User is not found");