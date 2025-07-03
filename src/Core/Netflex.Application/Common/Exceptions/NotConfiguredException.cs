using Netflex.Shared.Exceptions;

namespace Netflex.Application.Common.Exceptions;

public class NotConfiguredException(string name)
    : InternalServerException($"{name} isn't configured.");