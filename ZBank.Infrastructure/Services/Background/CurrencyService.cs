using System.Globalization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using RestSharp;
using ZBank.Application.Common.Interfaces.Persistance;
using ZBank.Domain.CurrencyAggregate;

namespace ZBank.Infrastructure.Services.Background;
public class CurrencyService : BackgroundService
{
    private readonly ILogger<CurrencyService> _logger;
    private readonly IServiceScopeFactory _scopeFactory;

    public CurrencyService(ILogger<CurrencyService> logger, IServiceScopeFactory scopeFactory)
    {
        _logger = logger;
        _scopeFactory = scopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation($"Executing CurrencyService at {DateTime.UtcNow}");

            try
            {
                using var scope = _scopeFactory.CreateScope();
                var currencyRepository = scope.ServiceProvider.GetRequiredService<ICurrencyRepository>();
                var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

                var currencies = await GetAllCurrencies();

                if (currencies is not null)
                {
                    await UpdateCurrencies(currencies, currencyRepository, unitOfWork);
                }
                else
                {
                    _logger.LogWarning("No currencies found. Check previous logs for more details.");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred in CurrencyService: {ex.Message}");
            }

            await Task.Delay(20000, stoppingToken);
        }
    }

    private async Task<List<Currency>?> GetAllCurrencies()
    {
        _logger.LogInformation("Getting all currencies");
        
        var responseContent = await FetchDataFromApi();
        if (responseContent == null)
        {
            _logger.LogWarning("Failed to fetch currencies from the API.");
            return null;
        }

        var currencies = ParseCurrencies(responseContent);
        _logger.LogInformation($"Successfully parsed {currencies.Count} currencies.");

        return currencies;
    }

    private async Task<string?> FetchDataFromApi()
    {
        string url = "https://www.okx.com/api/v5/market/tickers?instType=SPOT";

        var client = new RestClient(url);
        var request = new RestRequest();

        try
        {
            var response = await client.GetAsync(request);

            if (response is { IsSuccessful: true, StatusCode: System.Net.HttpStatusCode.OK })
            {
                _logger.LogInformation("Successfully retrieved data from the API.");
                return response.Content;
            }

            _logger.LogWarning($"Failed to fetch data. Status: {response.StatusCode}");
            return null;
        }
        catch (Exception ex)
        {
            _logger.LogError($"An error occurred while fetching data from the API. Error: {ex.Message}");
            return null;
        }
    }

    private List<Currency> ParseCurrencies(string jsonResponse)
    {
        List<Currency> currencies = new();
        var parsedResponse = JObject.Parse(jsonResponse);
        var tickers = parsedResponse["data"];

        foreach (var ticker in tickers!)
        {
            string instrument = ticker["instId"]!.ToString();

            if (IsValidInstrument(instrument))
            {
                decimal price = decimal.Parse(ticker["last"]!.ToString(), CultureInfo.InvariantCulture);
                string symbol = instrument.Split('-')[0];

                var currency = Currency.Create(symbol, price);
                currencies.Add(currency);
            }
        }

        return currencies;
    }

    private bool IsValidInstrument(string instrument)
    {
        return instrument.EndsWith("-USDT") || instrument.EndsWith("-USDC");
    }

    private async Task UpdateCurrencies(
        List<Currency> currencies,
        ICurrencyRepository currencyRepository,
        IUnitOfWork unitOfWork)
    {
        _logger.LogInformation("Updating currencies...");

        var existingCurrencies = await currencyRepository.GetAllCurrencies();
        var existingCurrencyMap = existingCurrencies.ToDictionary(x => x.Symbol, x => x);

        foreach (var newCurrency in currencies)
        {
            if (existingCurrencyMap.TryGetValue(newCurrency.Symbol, out var existingCurrency))
            {
                if (existingCurrency.Price != newCurrency.Price)
                {
                    existingCurrency.UpdatePrice(newCurrency.Price);
                    currencyRepository.UpdateCurrency(existingCurrency);
                }
            }
            else
            {
                currencyRepository.AddCurrency(newCurrency);
            }
        }

        await unitOfWork.SaveChangesAsync();

        _logger.LogInformation("Currencies updated successfully.");
    }
}
