using System.Security.Claims;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Authorization;
using ZBank.Application.Common.Interfaces.Services;
using ZBank.Contracts.Currencies;
using ZBank.Domain.UserAggregate.ValueObjects;

namespace ZBank.API.Hubs;

public interface ICurrencyClient
{
    Task ReceiveMessage(string message);
    
    Task ReceiveCurrencyUpdates(GetCurrencyUpdatesResponse currencyUpdates);
}

[Authorize]
public class CurrencyHub : Hub<ICurrencyClient>
{
    private readonly IUserConnectionManager _connectionManager;

    public CurrencyHub(IUserConnectionManager connectionManager)
    {
        _connectionManager = connectionManager;
    }
    
    public async Task JoinTeamGroup(string teamId)
    {
        var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (userId != null && Guid.TryParse(userId, out var validUserId))
        {
            var currentTeam = _connectionManager.GetActiveTeam(UserId.Create(validUserId));
            if (currentTeam != null)
            {
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"team-{currentTeam}");
                await Clients.Caller.ReceiveMessage($"You have been removed from group team-{currentTeam}");   
            }

            await Groups.AddToGroupAsync(Context.ConnectionId, $"team-{teamId}");
            _connectionManager.SetActiveTeam(UserId.Create(validUserId), teamId);
            
            await Clients.Caller.ReceiveMessage($"Joined team group successfully. Group name: team-{teamId}. User ID: {validUserId}");
        }
    }

    public async Task LeaveTeamGroup(string teamId)
    {
        var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (userId != null && Guid.TryParse(userId, out var validUserId))
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"team-{teamId}");
            await Clients.Caller.ReceiveMessage($"Leaved team group successfully. Group name: team-{teamId}. User ID: {validUserId}");
        }
    }
    
    public override async Task OnConnectedAsync()
    {
        var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (userId != null && Guid.TryParse(userId, out var validUserId))
        {
            _connectionManager.AddConnection(UserId.Create(validUserId), Context.ConnectionId);
            await Clients.Caller.ReceiveMessage($"Connected successfully. You are now receiving currency updates. User ID: {validUserId}");
        }
    }
    
    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (userId != null && Guid.TryParse(userId, out var validUserId))
        {
            var userIdObj = UserId.Create(validUserId);

            _connectionManager.RemoveConnection(Context.ConnectionId);
        
            var remainingConnections = _connectionManager.GetConnections(userIdObj);

            if (remainingConnections == null || remainingConnections.Count == 0)
            {
                var teamId = _connectionManager.GetActiveTeam(userIdObj);
                if (teamId != null)
                {
                    await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"team-{teamId}");
                    _connectionManager.RemoveActiveTeam(userIdObj);
                }
            }
        }

        await base.OnDisconnectedAsync(exception);
    }
}