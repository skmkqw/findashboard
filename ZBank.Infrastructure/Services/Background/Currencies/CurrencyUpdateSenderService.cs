using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ZBank.Application.Common;
using ZBank.Application.Common.Interfaces.Persistance;
using ZBank.Application.Common.Interfaces.Services;
using ZBank.Application.Common.Interfaces.Services.Currencies;
using ZBank.Domain.CurrencyAggregate;
using ZBank.Domain.TeamAggregate.ValueObjects;

namespace ZBank.Infrastructure.Services.Background.Currencies;

public class CurrencyUpdateSenderService : PriceUpdateSenderService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<CurrencyUpdateSenderService> _logger;
    private readonly TimeSpan _updateInterval = TimeSpan.FromSeconds(65);

    public CurrencyUpdateSenderService(IServiceScopeFactory scopeFactory,
        ILogger<CurrencyUpdateSenderService> logger,
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

    protected override async Task NotifyClients(IEnumerable<string> groupNames)
    {
        using var scope = _scopeFactory.CreateScope();
        var currencyUpdateSender = scope.ServiceProvider.GetRequiredService<ICurrencyUpdateSender>();

        foreach (var groupName in groupNames)
        {
            var currencies = await GetGroupCurrencies(groupName);

            _logger.LogInformation("Broadcasting currency updates");
            await currencyUpdateSender.SendUpdateAsync(currencies, groupName);
        }
    }
}