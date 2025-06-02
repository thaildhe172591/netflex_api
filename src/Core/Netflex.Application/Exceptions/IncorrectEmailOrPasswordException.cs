using Netflex.Shared.Exceptions;

namespace Netflex.Application.Exceptions;

public class IncorrectEmailOrPasswordException()
    : BadRequestException("Email or Password is incorrect");