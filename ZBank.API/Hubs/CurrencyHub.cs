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
    
    public override async Task OnConnectedAsync()
    {
        var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        if (userId != null && Guid.TryParse(userId, out var validUserId))
        {
            _connectionManager.AddConnection(UserId.Create(validUserId), Context.ConnectionId);
            await Clients.Caller.ReceiveMessage($"Connected successfully. You are now receiving currency updates. User ID: {userId}");
        }
    }
    
    public override Task OnDisconnectedAsync(Exception? exception)
    {
        _connectionManager.RemoveConnection(Context.ConnectionId);
        return base.OnDisconnectedAsync(exception);
    }
}