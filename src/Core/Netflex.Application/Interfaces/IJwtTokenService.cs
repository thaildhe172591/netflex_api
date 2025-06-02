using Netflex.Domain.Entities;

namespace Netflex.Application.Interfaces;

public static class CustomClaimNames
{
    public const string Version = "ver";
}

public interface IJwtTokenService
{
    string GenerateJwt(User user, string sessionId);
    Task AddToBlacklistAsync(string accessJti);
    Task<bool> IsRevokedAsync(string accessJti);
}
