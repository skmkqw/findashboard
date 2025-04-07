using System.Security.Claims;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Authorization;
using ZBank.API.Hubs.Common;
using ZBank.Contracts.Currencies;
using ZBank.Contracts.Wallets;
using ZBank.Domain.UserAggregate.ValueObjects;
using IGroupManager = ZBank.Application.Common.Interfaces.Services.IGroupManager;

namespace ZBank.API.Hubs;

public interface IPriceClient : IHubClient
{
    Task ReceiveCurrencyUpdates(GetCurrencyUpdatesResponse currencyUpdates);
    
    Task ReceiveWalletUpdates(GetWalletUpdatesResponse walletUpdates);
}

[Authorize]
public class PriceHub : Hub<IPriceClient>
{
    private readonly IGroupManager _groupManager;

    public PriceHub(IGroupManager groupManager)
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
        var userIdClaim = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (userIdClaim is null || !Guid.TryParse(userIdClaim, out var validUserId))
        {
            await Clients.Caller.ReceiveMessage("Invalid user identity. Cannot join group.");
            return;
        }

        var userId = UserId.Create(validUserId);
        
        var joinGroupResult = await _groupManager.TryAddUserToGroupAsync(userId, Context.ConnectionId, teamId);
        if (joinGroupResult.IsError)
        {
            await Clients.Caller.ReceiveMessage($"Access denied: {joinGroupResult.FirstError.Code}-{joinGroupResult.FirstError.Description}");
            return;
        }

        await Groups.AddToGroupAsync(Context.ConnectionId, $"team-{teamId}");
    
        await Clients.Caller.ReceiveMessage($"Joined team group successfully. Group name: team-{teamId}. User ID: {validUserId}");
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
