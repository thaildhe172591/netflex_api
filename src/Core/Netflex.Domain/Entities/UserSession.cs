namespace Netflex.Domain.Entities;

public class UserSession : Entity<string>
{
    public required string UserId { get; set; }
    public required string DeviceId { get; set; }
    public string? DeviceInfo { get; set; }
    public string? IpAddress { get; set; }
    public required HashString RefreshHash { get; set; }
    public required DateTime ExpiresAt { get; set; }
    public bool IsRevoked { get; set; }

    public static UserSession Create(string id, string deviceId,
        HashString refreshHash, string userId, DateTime expiresAt)
        => new()
        {
            Id = id,
            DeviceId = deviceId,
            UserId = userId,
            RefreshHash = refreshHash,
            ExpiresAt = expiresAt
        };

    public static UserSession Create(string id, string deviceId, HashString refreshHash,
        string userId, string? deviceInfo, string? ipAddress, DateTime expiresAt)
    {
        var session = Create(id, deviceId, refreshHash, userId, expiresAt);
        session.DeviceInfo = deviceInfo;
        session.IpAddress = ipAddress;
        return session;
    }

    public bool IsValid(DateTime now) => !IsRevoked && now < ExpiresAt;

    public void Revoke()
    {
        IsRevoked = true;
        ExpiresAt = DateTime.UtcNow;
    }
}