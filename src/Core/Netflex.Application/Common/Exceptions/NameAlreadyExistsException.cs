using Netflex.Shared.Exceptions;

namespace Netflex.Application.Common.Exceptions;

public class NameAlreadyExistsException(string name)
    : BadRequestException($"{name} is already exists");