using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using ZBank.API.Hubs.Common;
using ZBank.Application.Common.Interfaces.Services;
using ZBank.Contracts.Notifications.GetUserNotifications;
using ZBank.Domain.UserAggregate.ValueObjects;

namespace ZBank.API.Hubs;

public interface INotificationClient : IHubClient
{
    Task ReceiveInformationNotification(InformationNotificationResponse notification);
    
    Task ReceiveTeamInviteNotification(TeamInviteNotificationResponse notification);
}


[Authorize]
public class NotificationHub : Hub<INotificationClient>
{
    private readonly IUserConnectionManager _connectionManager;

    public NotificationHub(IUserConnectionManager connectionManager)
    {
        _connectionManager = connectionManager;
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

    public override Task OnDisconnectedAsync(Exception? exception)
    {
        _connectionManager.RemoveConnection(Context.ConnectionId);
        return base.OnDisconnectedAsync(exception);
    }
}
