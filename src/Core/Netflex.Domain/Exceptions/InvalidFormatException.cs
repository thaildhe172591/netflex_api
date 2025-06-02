namespace Netflex.Domain.Exceptions;

public class InvalidFormatException(string name)
    : ArgumentException($"Invalid {name} format.");