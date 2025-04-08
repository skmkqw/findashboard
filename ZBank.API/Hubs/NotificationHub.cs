using Microsoft.AspNetCore.Authorization;
using ZBank.API.Hubs.Common;
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
        
        await Clients.Caller.ReceiveMessage($"Connected successfully. Join a group to receive notifications. User ID: {userId.Value}");
    }
    
    public async Task JoinTeamGroup(string teamId) => await TryJoinGroupAsync(teamId);
    
    public async Task LeaveTeamGroup(string teamId) => await LeaveGroupAsync(teamId);

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var userId = GetUserId();
        if (userId is null)
        {
            await Clients.Caller.ReceiveMessage("Connection failed. User identity cannot be determined");
            return;
        }
    
        var groups = GroupManager.GetAllGroups();
    
        groups.ForEach(groupId => GroupManager.RemoveUserFromGroup(userId, Context.ConnectionId, groupId));
    }
}
