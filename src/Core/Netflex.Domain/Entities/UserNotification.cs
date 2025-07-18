namespace Netflex.Domain.Entities;

public class UserNotification : IEntity
{
    public required string UserId { get; set; }
    public required long NotificationId { get; set; }
    public bool HaveRead { get; set; }
    public static UserNotification Create(string userId, long notificationId, bool haveRead = false)
        => new() { UserId = userId, NotificationId = notificationId, HaveRead = haveRead };
    public void MarkAsRead()
    {
        HaveRead = true;
    }
}