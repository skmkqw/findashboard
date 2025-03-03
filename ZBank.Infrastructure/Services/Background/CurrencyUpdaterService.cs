using System.Globalization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using RestSharp;
using ZBank.Application.Common.Interfaces.Persistance;
using ZBank.Domain.CurrencyAggregate;

namespace ZBank.Infrastructure.Services.Background;

public class CurrencyUpdaterService : BackgroundService
{
    private readonly string _url = "https://www.okx.com/api/v5/market/tickers?instType=SPOT";
    
    private readonly TimeSpan _refreshInterval = TimeSpan.FromMinutes(1);
    
    private readonly ILogger<CurrencyUpdaterService> _logger;
    
    private readonly IServiceScopeFactory _scopeFactory;

    public CurrencyUpdaterService(ILogger<CurrencyUpdaterService> logger, IServiceScopeFactory scopeFactory)
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
                await UpdateCurrenciesInBulk();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating currencies");
            }
        }
    }

    private async Task UpdateCurrenciesInBulk()
    {
        _logger.LogInformation($"Updating currencies at {DateTime.UtcNow}");

        var currencies = await FetchAndParseCurrencies();
        if (currencies == null || !currencies.Any())
        {
            _logger.LogWarning("No currencies fetched. Skipping update.");
            return;
        }

        using var scope = _scopeFactory.CreateScope();
        var currencyRepository = scope.ServiceProvider.GetRequiredService<ICurrencyRepository>();
        var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

        try
        {
            await currencyRepository.ReplaceAllCurrenciesAsync(currencies);
            await unitOfWork.SaveChangesAsync();

            _logger.LogInformation($"Successfully updated {currencies.Count} currencies");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to update currencies in bulk");
            throw;
        }
    }

    private async Task<List<Currency>?> FetchAndParseCurrencies()
    {
        var responseContent = await FetchDataFromApi();
        if (string.IsNullOrEmpty(responseContent))
        {
            return null;
        }

        return ParseCurrencies(responseContent);
    }

    private async Task<string?> FetchDataFromApi()
    {
        var client = new RestClient(_url);
        var request = new RestRequest();

        try
        {
            var response = await client.ExecuteGetAsync(request);

            return response.IsSuccessful && response.StatusCode == System.Net.HttpStatusCode.OK 
                ? response.Content 
                : null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error fetching currency data from API");
            return null;
        }
    }

    private List<Currency> ParseCurrencies(string jsonResponse)
    {
        try
        {
            var parsedResponse = JObject.Parse(jsonResponse);
            var tickers = parsedResponse["data"];

            return tickers?
                .Where(ticker => IsValidInstrument(ticker["instId"]!.ToString()))
                .Select(ticker => Currency.Create(
                    symbol: ticker["instId"]!.ToString().Replace('-', '/'),
                    price: decimal.Parse(ticker["last"]!.ToString(), CultureInfo.InvariantCulture),
                    openPrice: decimal.Parse(ticker["open24h"]!.ToString(), CultureInfo.InvariantCulture)
                ))
                .ToList() ?? new List<Currency>();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error parsing currencies from JSON");
            return new List<Currency>();
        }
    }

    private bool IsValidInstrument(string instrument)
    {
        return instrument.EndsWith("-USDT") || instrument.EndsWith("-USDC");
    }
}