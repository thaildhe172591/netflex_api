namespace Netflex.Application.DTOs;

public record FollowDto
{
    public required string TargetId { get; init; }
    public required string TargetType { get; init; }
    public required string UserId { get; init; }
    public required DateTime CreatedAt { get; init; }
}