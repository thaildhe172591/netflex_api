namespace Netflex.Domain.Entities;

public class Review : IEntity
{
    public required string TargetId { get; set; }
    public required TargetType TargetType { get; set; }
    public required string UserId { get; set; }
    public required Rating Rating { get; set; }
    public string? Comment { get; set; }
    public int LikeCount { get; set; }
    public required DateTime CreatedAt { get; set; }
    private Review() { }

    public static Review Create(string targetId, TargetType targetType, string userId, Rating rating,
        string? comment, DateTime? createdAt = default)
    {
        return new Review()
        {
            TargetId = targetId,
            TargetType = targetType,
            UserId = userId,
            Rating = rating,
            Comment = comment,
            CreatedAt = createdAt ?? DateTime.UtcNow
        };
    }

}