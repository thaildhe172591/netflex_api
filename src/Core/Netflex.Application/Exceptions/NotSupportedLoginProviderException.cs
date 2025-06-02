namespace Netflex.Application.Exceptions;

public class NotSupportedLoginProviderException()
: NotSupportedException($"Login provider is not currently supported");