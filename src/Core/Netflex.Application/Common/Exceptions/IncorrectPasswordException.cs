using Netflex.Shared.Exceptions;

namespace Netflex.Application.Common.Exceptions;

public class IncorrectPasswordException()
    : BadRequestException("Password is incorrect");