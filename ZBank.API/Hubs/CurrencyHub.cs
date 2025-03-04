using System.Security.Claims;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Authorization;
using ZBank.Contracts.Currencies;
using ZBank.Domain.UserAggregate.ValueObjects;
using IGroupManager = ZBank.Application.Common.Interfaces.Services.IGroupManager;

namespace ZBank.API.Hubs;

public interface ICurrencyClient
{
    Task ReceiveMessage(string message);
    
    Task ReceiveCurrencyUpdates(GetCurrencyUpdatesResponse currencyUpdates);
}

[Authorize]
public class CurrencyHub : Hub<ICurrencyClient>
{
    private readonly IGroupManager _groupManager;

    public CurrencyHub(IGroupManager groupManager)
    {
        _groupManager = groupManager;
    }
    
    public override async Task OnConnectedAsync()
    {
        var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (userId != null && Guid.TryParse(userId, out var validUserId))
        {
            await Clients.Caller.ReceiveMessage($"Connected successfully. Join group to receive currency updates. User ID: {validUserId}");
            return;
        }
        
        await Clients.Caller.ReceiveMessage("Connection failed. User identity cannot be determined");
    }
    
    public async Task JoinTeamGroup(string teamId)
    {
        var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (userId != null && Guid.TryParse(userId, out var validUserId))
        {
            _groupManager.AddUserToGroup(UserId.Create(validUserId), Context.ConnectionId, teamId);
            await Groups.AddToGroupAsync(Context.ConnectionId, $"team-{teamId}");
            
            await Clients.Caller.ReceiveMessage($"Joined team group successfully. Group name: team-{teamId}. User ID: {validUserId}");
        }
    }

    public async Task LeaveTeamGroup(string teamId)
    {
        var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (userId != null && Guid.TryParse(userId, out var validUserId))
        {
            _groupManager.RemoveUserFromGroup(UserId.Create(validUserId), Context.ConnectionId, teamId);
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"team-{teamId}");
            
            await Clients.Caller.ReceiveMessage($"Left team group successfully. Group name: team-{teamId}. User ID: {validUserId}");
        }
    }
}
