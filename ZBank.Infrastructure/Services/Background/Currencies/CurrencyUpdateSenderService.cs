using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ZBank.Application.Common.Interfaces.Persistance;
using ZBank.Application.Common.Interfaces.Services;
using ZBank.Application.Common.Interfaces.Services.Currencies;
using ZBank.Domain.CurrencyAggregate;
using ZBank.Domain.TeamAggregate.ValueObjects;

namespace ZBank.Infrastructure.Services.Background.Currencies;

public class CurrencyUpdateSenderService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly IGroupManager _groupManager;
    private readonly ILogger<CurrencyUpdateSenderService> _logger;
    private readonly TimeSpan _updateInterval = TimeSpan.FromMinutes(1);

    public CurrencyUpdateSenderService(IServiceScopeFactory scopeFactory,
        ILogger<CurrencyUpdateSenderService> logger,
        IGroupManager groupManager)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
        _groupManager = groupManager;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using PeriodicTimer timer = new(_updateInterval);
        while (await timer.WaitForNextTickAsync(stoppingToken))
        {
            try
            {
                await NotifyClients();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while sending currency updates");
            }
        }
    }

    private List<string> GetGroupNames() => _groupManager.GetAllGroups();

    private async Task<List<Currency>> GetGroupCurrencies(string groupName)
    {
        using var scope = _scopeFactory.CreateScope();
        var currencyRepository = scope.ServiceProvider.GetRequiredService<ICurrencyRepository>();

        _logger.LogInformation($"Getting currencies for group: {groupName}");

        var teamId = TeamId.Create(Guid.Parse(groupName));
        var currencies = await currencyRepository.GetTeamCurrenciesAsync(teamId);
        
        if (currencies.Count == 0)
            _logger.LogWarning($"No currencies found for group {groupName}");

        return currencies;
    }

    private async Task NotifyClients()
    {
        using var scope = _scopeFactory.CreateScope();
        var currencyUpdateSender = scope.ServiceProvider.GetRequiredService<ICurrencyUpdateSender>();

        foreach (var groupName in GetGroupNames())
        {
            var currencies = await GetGroupCurrencies(groupName);

            _logger.LogInformation("Broadcasting currency updates");
            await currencyUpdateSender.SendUpdateAsync(currencies, groupName);
        }
    }
}
