using Netflex.Shared.Exceptions;

namespace Netflex.Application.Common.Exceptions;

public class EmailAlreadyExistsException()
    : BadRequestException("Email is already exists");