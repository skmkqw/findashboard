using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ZBank.Application.Common;
using ZBank.Application.Common.Interfaces.Persistance;
using ZBank.Application.Common.Interfaces.Services;
using ZBank.Application.Common.Interfaces.Services.Wallets;
using ZBank.Domain.TeamAggregate.ValueObjects;
using ZBank.Domain.WalletAggregate;

namespace ZBank.Infrastructure.Services.Background.Wallets;

public class WalletUpdateSenderService : PriceUpdateSenderService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<WalletUpdateSenderService> _logger;
    private readonly TimeSpan _updateInterval = TimeSpan.FromSeconds(75);

    public WalletUpdateSenderService(IServiceScopeFactory scopeFactory,
        ILogger<WalletUpdateSenderService> logger,
        IGroupManager groupManager) : base(groupManager)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using PeriodicTimer timer = new(_updateInterval);
        while (await timer.WaitForNextTickAsync(stoppingToken))
        {
            try
            {
                var groupNames = GetGroupNames();
                await NotifyClients(groupNames);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while sending currency updates");
            }
        }
    }
    
    private async Task<List<Wallet>> GetGroupCurrencies(string groupName)
    {
        using var scope = _scopeFactory.CreateScope();
        var walletRepository = scope.ServiceProvider.GetRequiredService<IWalletRepository>();

        _logger.LogInformation($"Getting wallets for group: {groupName}");

        var teamId = TeamId.Create(Guid.Parse(groupName));
        var wallets = await walletRepository.GetTeamWalletsAsync(teamId);

        if (wallets.Count == 0)
            _logger.LogWarning($"No wallets found for group {groupName}");

        return wallets;
    }

    protected override async Task NotifyClients(IEnumerable<string> groupNames)
    {
        using var scope = _scopeFactory.CreateScope();
        var walletUpdateSender = scope.ServiceProvider.GetRequiredService<IWalletUpdateSender>();

        foreach (var groupName in groupNames)
        {
            var currencies = await GetGroupCurrencies(groupName);

            _logger.LogInformation("Broadcasting currency updates");
            await walletUpdateSender.SendUpdateAsync(currencies, groupName);
        }
    }
}