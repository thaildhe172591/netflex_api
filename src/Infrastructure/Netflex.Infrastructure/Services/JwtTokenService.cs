using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Netflex.Application.Interfaces;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Netflex.Domain.Entities;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;

namespace Netflex.Infrastructure.Services;

public record JwtConfig
{
    public string Key { get; init; } = string.Empty;
    public double ExpiresInMinutes { get; init; }
    public string Issuer { get; init; } = string.Empty;
    public string Audience { get; init; } = string.Empty;
}

public class JwtTokenService(IOptions<JwtConfig> options, IDistributedCache distributedCache)
    : IJwtTokenService
{
    private readonly IDistributedCache _distributedCache = distributedCache;
    private readonly JwtConfig _jwtConfig = options.Value;

    public string GenerateJwt(User user, string sessionId)
    {

        var accessJti = Guid.NewGuid().ToString();
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Jti, accessJti),
            new(ClaimTypes.NameIdentifier, user.Id),
            new(JwtRegisteredClaimNames.Email, user.Email.Value),
            new(JwtRegisteredClaimNames.EmailVerified, user.EmailConfirmed.ToString()),
            new(CustomClaimNames.Version, user.Version.ToString()),
            new(JwtRegisteredClaimNames.Sid, sessionId)
        };

        user.Roles.ToList().ForEach(role => claims.Add(new(ClaimTypes.Role, role.Name)));

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfig.Key));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new(claims),
            Expires = DateTime.Now.AddMinutes(_jwtConfig.ExpiresInMinutes),
            SigningCredentials = credentials,
            Issuer = _jwtConfig.Issuer,
            Audience = _jwtConfig.Audience
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    public async Task AddToBlacklistAsync(string accessJti)
    {
        await _distributedCache.SetStringAsync($"token:{accessJti}", "1", new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(_jwtConfig.ExpiresInMinutes)
        });
    }

    public async Task<bool> IsRevokedAsync(string accessJti)
    {
        return await _distributedCache.GetStringAsync($"token:{accessJti}") != null;
    }
}