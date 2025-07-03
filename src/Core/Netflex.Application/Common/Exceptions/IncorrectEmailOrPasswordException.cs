using Netflex.Shared.Exceptions;

namespace Netflex.Application.Common.Exceptions;

public class IncorrectEmailOrPasswordException()
    : BadRequestException("Email or Password is incorrect");