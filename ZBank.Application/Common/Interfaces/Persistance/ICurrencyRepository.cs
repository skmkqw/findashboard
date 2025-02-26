using ZBank.Domain.CurrencyAggregate;
using ZBank.Domain.CurrencyAggregate.ValueObjects;

namespace ZBank.Application.Common.Interfaces.Persistance;

public interface ICurrencyRepository
{
    Task ReplaceAllCurrenciesAsync(List<Currency> newCurrencies);
    
    Task<Currency?> GetCurrencyBySymbol(CurrencyId symbol);

}