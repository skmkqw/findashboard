using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using ZBank.Application.Common.Interfaces.Services;
using ZBank.Contracts.Notifications.GetUserNotifications;
using ZBank.Domain.UserAggregate.ValueObjects;

namespace ZBank.API.Hubs;

public interface INotificationClient
{
    Task ReceiveMessage(string message);
    
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
        var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (userId != null && Guid.TryParse(userId, out var validUserId))
        {
            _connectionManager.AddConnection(UserId.Create(validUserId), Context.ConnectionId);
            await Clients.Caller.ReceiveMessage($"Connected successfully. User ID: {userId}");
        }
    }

    public override Task OnDisconnectedAsync(Exception? exception)
    {
        _connectionManager.RemoveConnection(Context.ConnectionId);
        return base.OnDisconnectedAsync(exception);
    }
}
