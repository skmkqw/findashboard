using Microsoft.Extensions.Hosting;
using ZBank.Application.Common.Interfaces.Services;

namespace ZBank.Application.Common;

public abstract class PriceUpdateSenderService : BackgroundService
{
    private readonly IGroupManager _groupManager;

    protected PriceUpdateSenderService(IGroupManager groupManager)
    {
        _groupManager = groupManager;
    }

    protected List<string> GetGroupNames() => _groupManager.GetAllGroups();

    protected abstract Task NotifyClients(IEnumerable<string> groupNames);
}