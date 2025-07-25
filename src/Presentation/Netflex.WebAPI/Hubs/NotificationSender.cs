using Microsoft.AspNetCore.SignalR;
using Netflex.Application.Interfaces;

namespace Netflex.WebAPI.Hubs;

public class NotificationSender : INotificationSender
{
    private readonly IHubContext<NotificationHub, INotificationClient> _hubContext;
    private readonly ConnectionManager _connectionManager;

    public NotificationSender(IHubContext<NotificationHub, INotificationClient> hubContext, ConnectionManager connectionManager)
    {
        _connectionManager = connectionManager;
        _hubContext = hubContext;
    }
    public async Task SendNotificationAsync(Message message)
    {
        var connectionIds = message.SendTo.SelectMany(_connectionManager.GetConnections).Distinct();
        if (connectionIds == null || !connectionIds.Any()) return;
        await _hubContext.Clients.Clients(connectionIds).ReceiveNotification(message.Content);
    }
}