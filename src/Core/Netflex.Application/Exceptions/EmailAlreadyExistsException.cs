using Netflex.Shared.Exceptions;

namespace Netflex.Application.Exceptions;

public class EmailAlreadyExistsException()
    : BadRequestException("Email is already exists");