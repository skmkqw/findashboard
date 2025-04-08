using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using ZBank.API.Hubs.Common;
using ZBank.Application.Common.Interfaces.Services;
using ZBank.Contracts.Notifications.GetUserNotifications;
using ZBank.Application.Common.Interfaces.Services;

namespace ZBank.API.Hubs;

public interface INotificationClient : IHubClient
{
    Task ReceiveInformationNotification(InformationNotificationResponse notification);
    
    Task ReceiveTeamInviteNotification(TeamInviteNotificationResponse notification);
}


[Authorize]
public class NotificationHub : HubBase<INotificationClient>
{
    public NotificationHub(IGroupManager groupManager) : base(groupManager)
    {
    }
    
    public override async Task OnConnectedAsync()
    {
        var userId = GetUserId();
        if (userId is null)
        {
            await Clients.Caller.ReceiveMessage("Connection failed. User identity cannot be determined");
            return;
        }
        
        await Clients.Caller.ReceiveMessage($"Connected successfully. Join a group to receive notifications. User ID: {userId}");
    }
    
    public async Task JoinTeamGroup(string teamId) => await TryJoinGroupAsync(teamId);
    
    public async Task LeaveTeamGroup(string teamId) => await LeaveGroupAsync(teamId);

    public override Task OnDisconnectedAsync(Exception? exception)
    {
        _connectionManager.RemoveConnection(Context.ConnectionId);
        return base.OnDisconnectedAsync(exception);
    }
}
