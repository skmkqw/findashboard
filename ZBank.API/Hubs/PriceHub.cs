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
        var userId = GetUserId();
        if (userId is null)
        {
            await Clients.Caller.ReceiveMessage("Connection failed. User identity cannot be determined");
            return;
        }

        await Clients.Caller.ReceiveMessage(
            $"Connected successfully. Join group to receive currency updates. User ID: {userId.Value}");
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