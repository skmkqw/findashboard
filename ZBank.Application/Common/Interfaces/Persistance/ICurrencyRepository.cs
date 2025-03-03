using ZBank.Domain.CurrencyAggregate;
using ZBank.Domain.CurrencyAggregate.ValueObjects;

namespace ZBank.Application.Common.Interfaces.Persistance;

public interface ICurrencyRepository
{
    Task<List<Currency>> GetAllCurrenciesAsync();
    
    Task ReplaceAllCurrenciesAsync(List<Currency> newCurrencies);
    
    Task<Currency?> GetCurrencyBySymbolAsync(CurrencyId symbol);

}