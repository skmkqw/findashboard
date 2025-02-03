using ZBank.Domain.CurrencyAggregate;
using ZBank.Domain.CurrencyAggregate.ValueObjects;

namespace ZBank.Application.Common.Interfaces.Persistance;

public interface ICurrencyRepository
{
    Task<List<Currency>> GetAllCurrencies();
    
    Task<Currency?> GetCurrencyById(CurrencyId id);

    Task ReplaceAllCurrenciesAsync(List<Currency> newCurrencies);
    
    Task<Currency?> GetCurrencyBySymbol(string symbol);

}