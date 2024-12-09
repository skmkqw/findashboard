using Microsoft.AspNetCore.SignalR;
using ZBank.Contracts.Notifications.GetUserNotifications;

namespace ZBank.API.Hubs;

public interface INotificationClient
{
    Task ReceiveMessage(string message);
    
    Task ReceiveInformationNotification(InformationNotificationResponse notification);
    
    Task ReceiveTeamInviteNotification(TeamInviteNotificationResponse notification);
}

public class NotificationHub : Hub<INotificationClient>
{
    public override async Task OnConnectedAsync()
    {
        await Clients.All.ReceiveMessage($"{Context.ConnectionId} has connected successfully");
    }
}