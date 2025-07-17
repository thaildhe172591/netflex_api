namespace Netflex.Application.DTOs;

public record NotificationDto
{
    public long Id { get; set; }
    public string? Title { get; set; }
    public string? Content { get; set; }
    public DateTime? CreatedAt { get; set; }
}