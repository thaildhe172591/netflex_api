using Netflex.Domain.ValueObjects;

namespace Netflex.Application.Interfaces;

public interface ISocialLoginServiceFactory
{
    ISocialLoginService? GetByProvider(LoginProvider provider);
}