namespace Netflex.Domain.Entities;

public class Notification : Entity<long>
{
    public required string Title { get; set; }
    public string? Content { get; set; }
    public required DateTime CreatedAt { get; set; }
    private Notification() { }
    public static Notification Create(string title, string? content = default,
        DateTime? createdAt = default)
    {
        return new()
        {
            Title = title,
            Content = content,
            CreatedAt = createdAt ?? DateTime.UtcNow
        };
    }
}