using System.Security.Claims;
using Microsoft.AspNetCore.SignalR;
using ZBank.Domain.UserAggregate.ValueObjects;
using IGroupManager = ZBank.Application.Common.Interfaces.Services.IGroupManager;

namespace ZBank.API.Hubs.Common;

public abstract class HubBase<TClient> : Hub<TClient> where TClient : class, IHubClient
{
    protected readonly IGroupManager GroupManager;

    protected HubBase(IGroupManager groupManager)
    {
        GroupManager = groupManager;
    }

    protected UserId? GetUserId()
    {
        var userId = Context.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return Guid.TryParse(userId, out var guid) ? UserId.Create(guid) : null;
    }

    protected async Task TryJoinGroupAsync(string groupId)
    {
        var userId = GetUserId();
        if (userId is null)
        {
            await Clients.Caller.ReceiveMessage("Invalid user identity.");
            return;
        }

        var result = await GroupManager.TryAddUserToGroupAsync(userId, Context.ConnectionId, groupId);
        if (result.IsError)
        {
            await Clients.Caller.ReceiveMessage($"Access denied: {result.FirstError.Description}");
            return;
        }

        await Groups.AddToGroupAsync(Context.ConnectionId, $"team-{groupId}");
        await Clients.Caller.ReceiveMessage($"Joined team group: team-{groupId}");
    }

    protected async Task LeaveGroupAsync(string groupId)
    {
        var userId = GetUserId();
        if (userId is null)
        {
            await Clients.Caller.ReceiveMessage("Invalid user identity.");
            return;
        }

        GroupManager.RemoveUserFromGroup(userId, Context.ConnectionId, groupId);
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"team-{groupId}");
        await Clients.Caller.ReceiveMessage(
            $"Left team group successfully. Group name: team-{groupId}. User ID: {userId}");
    }
}