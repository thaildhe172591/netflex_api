namespace Netflex.Domain.Entities;

public class Follow : IEntity
{
    public required string TargetId { get; set; }
    public required TargetType TargetType { get; set; }
    public required string UserId { get; set; }
    public required DateTime CreatedAt { get; set; }
    public static Follow Create(string targetId, TargetType targetType, string userId, DateTime? createdAt = default)
    {
        return new Follow()
        {
            TargetId = targetId,
            TargetType = targetType,
            UserId = userId,
            CreatedAt = createdAt ?? DateTime.UtcNow
        };
    }
}