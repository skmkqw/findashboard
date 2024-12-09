using Microsoft.AspNetCore.SignalR;

namespace ZBank.Application.Hubs;

public interface INotificationClient
{
    Task ReceiveNotification(string content, string sender, Guid notificationId);
}

public class NotificationHub : Hub<INotificationClient>
{
    public override async Task OnConnectedAsync()
    {
        await Clients.All.ReceiveNotification($"The current time is: {DateTime.UtcNow}",Context.ConnectionId, Guid.NewGuid());
    }

    public async Task SendMessage(string message)
    {
        await Clients.All.ReceiveNotification($"I heard that {message}, but the current time is: {DateTime.UtcNow}",Context.ConnectionId, Guid.NewGuid());
    }
}