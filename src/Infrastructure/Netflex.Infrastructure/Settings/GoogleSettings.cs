namespace Netflex.Infrastructure.Settings;

public record GoogleSettings
{
    public string ClientId { get; init; } = string.Empty;
    public string ClientSecret { get; init; } = string.Empty;
    public string UserInfoUrl { get; init; } = string.Empty;
    public string CredentialsFile { get; init; } = string.Empty;
    public string BucketName { get; init; } = string.Empty;
};