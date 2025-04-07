using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using ZBank.API.Hubs.Common;
using ZBank.Contracts.Currencies;
using ZBank.Contracts.Wallets;
using IGroupManager = ZBank.Application.Common.Interfaces.Services.IGroupManager;

namespace ZBank.API.Hubs;

public interface IPriceClient : IHubClient
{
    Task ReceiveCurrencyUpdates(GetCurrencyUpdatesResponse currencyUpdates);
    
    Task ReceiveWalletUpdates(GetWalletUpdatesResponse walletUpdates);
}

[Authorize]
public class PriceHub : HubBase<IPriceClient>
{
    public PriceHub(IGroupManager groupManager) : base(groupManager)
    {
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
}
