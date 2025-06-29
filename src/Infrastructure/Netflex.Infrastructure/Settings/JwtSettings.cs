namespace Netflex.Infrastructure.Settings;

public record JwtSettings
{
    public string Key { get; init; } = string.Empty;
    public double ExpiresInMinutes { get; init; }
    public string Issuer { get; init; } = string.Empty;
    public string Audience { get; init; } = string.Empty;
}