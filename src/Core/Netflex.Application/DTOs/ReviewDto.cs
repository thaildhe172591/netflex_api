namespace Netflex.Application.DTOs;

public record ReviewDto
{
    public required string TargetId { get; init; }
    public required string TargetType { get; init; }
    public required string UserId { get; init; }
    public required int Rating { get; init; }
    public string? Comment { get; init; }
    public int LikeCount { get; init; }
    public required DateTime CreatedAt { get; init; }
}