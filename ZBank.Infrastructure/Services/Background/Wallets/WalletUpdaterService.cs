using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ZBank.Application.Common.Interfaces.Persistance;
using ZBank.Domain.CurrencyAggregate;
using ZBank.Domain.WalletAggregate;

namespace ZBank.Infrastructure.Services.Background.Wallets;

public class WalletUpdaterService : BackgroundService
{
    private IEnumerable<Currency>? _currencies;
    
    private readonly IServiceScopeFactory _scopeFactory;

    private readonly ILogger<WalletUpdaterService> _logger;
    
    private readonly TimeSpan _refreshInterval = TimeSpan.FromSeconds(70);

    public WalletUpdaterService(ILogger<WalletUpdaterService> logger, IServiceScopeFactory scopeFactory)
    {
        _logger = logger;
        _scopeFactory = scopeFactory;
    }
    
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        using PeriodicTimer timer = new(_refreshInterval);

        while (await timer.WaitForNextTickAsync(stoppingToken))
        {
            try
            {
                await RefreshCurrencies();
                await UpdateWalletsTotalAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating wallets");
            }
        }
    }

    private async Task RefreshCurrencies()
    {
        using var scope = _scopeFactory.CreateScope();
        var currencyRepository = scope.ServiceProvider.GetRequiredService<ICurrencyRepository>();
        _currencies = await currencyRepository.GetAllAsync();
    }
    
    private async Task UpdateWalletsTotalAsync()
    {
        using var scope = _scopeFactory.CreateScope();

        var walletRepository = scope.ServiceProvider.GetRequiredService<IWalletRepository>();
        var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

        var wallets = await walletRepository.GetAllAsync();

        foreach (var wallet in wallets)
        {
            UpdateWalletBalances(wallet);
            wallet.UpdateTotal();
        }

        await unitOfWork.SaveChangesAsync();
    }
    
    private void UpdateWalletBalances(Wallet wallet)
    {
        foreach (var balance in wallet.Balances)
        {
            var currency = _currencies!.FirstOrDefault(c => c.Id == balance.CurrencyId);

            if (currency is null)
            {
                _logger.LogWarning("Currency {CurrencyId} not found while updating balance.", balance.CurrencyId);
                continue;
            }

            var worth = balance.Amount * currency.Price;
            balance.UpdateTotal(worth);
        }
    }
}