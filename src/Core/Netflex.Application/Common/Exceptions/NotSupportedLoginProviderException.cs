namespace Netflex.Application.Common.Exceptions;

public class NotSupportedLoginProviderException()
: NotSupportedException($"Login provider is not currently supported");