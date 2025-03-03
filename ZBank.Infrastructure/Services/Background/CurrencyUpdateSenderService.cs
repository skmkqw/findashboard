using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ZBank.Application.Common.Interfaces.Persistance;
using ZBank.Application.Common.Interfaces.Services.Currencies;

namespace ZBank.Infrastructure.Services.Background;

public class CurrencyUpdateSenderService : BackgroundService
{
    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<CurrencyUpdateSenderService> _logger;
    private readonly TimeSpan _updateInterval = TimeSpan.FromMinutes(1);

    public CurrencyUpdateSenderService(IServiceScopeFactory scopeFactory, ILogger<CurrencyUpdateSenderService> logger)
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
                await NotifyClients();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while sending currency updates");
            }
        }
    }

    private async Task NotifyClients()
    {
        using var scope = _scopeFactory.CreateScope();
        var currencyRepository = scope.ServiceProvider.GetRequiredService<ICurrencyRepository>();
        var currencyUpdateNotifier = scope.ServiceProvider.GetRequiredService<ICurrencyUpdateSender>();

        var currencies = await currencyRepository.GetAllCurrenciesAsync();

        if (currencies.Count == 0)
        {
            _logger.LogWarning("No currencies found to notify clients.");
            return;
        }

        _logger.LogInformation("Broadcasting currency updates.");
        await currencyUpdateNotifier.SendUpdateAsync(currencies);
    }
}