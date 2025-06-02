using Netflex.Shared.Exceptions;

namespace Netflex.Application.Exceptions;

public class IncorrectPasswordException()
    : BadRequestException("Password is incorrect");