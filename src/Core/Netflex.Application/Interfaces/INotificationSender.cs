namespace Netflex.Application.Interfaces;

public record Message(IEnumerable<string> SendTo, string Content);
public interface INotificationSender
{
    Task SendNotificationAsync(Message message);
}