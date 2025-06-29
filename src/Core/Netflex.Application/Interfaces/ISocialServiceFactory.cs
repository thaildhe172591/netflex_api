using Netflex.Domain.ValueObjects;

namespace Netflex.Application.Interfaces;

public interface ISocialServiceFactory
{
    ISocialService? GetByProvider(LoginProvider provider);
}