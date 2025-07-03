using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Netflex.Domain.Entities;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using Netflex.Application.Interfaces;
using Netflex.Application.Common.Constants;

namespace Netflex.Infrastructure.Services;

public class JwtTokenService(IOptions<JwtSettings> options, IDistributedCache cache)
    : IJwtTokenService
{
    private readonly IDistributedCache _cache = cache;
    private readonly JwtSettings _settings = options.Value;

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

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.Key));
        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new(claims),
            Expires = DateTime.Now.AddMinutes(_settings.ExpiresInMinutes),
            SigningCredentials = credentials,
            Issuer = _settings.Issuer,
            Audience = _settings.Audience
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    public async Task AddToBlacklistAsync(string accessJti)
    {
        var key = string.Format(CacheKeys.TokenById, accessJti);
        await _cache.SetStringAsync(key, "1", new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(_settings.ExpiresInMinutes)
        });
    }

    public async Task<bool> IsRevokedAsync(string accessJti)
    {
        var key = string.Format(CacheKeys.TokenById, accessJti);
        return await _cache.GetStringAsync(key) != null;
    }
}