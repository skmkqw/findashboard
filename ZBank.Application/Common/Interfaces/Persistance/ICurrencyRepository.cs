using ZBank.Domain.CurrencyAggregate;
using ZBank.Domain.CurrencyAggregate.ValueObjects;

namespace ZBank.Application.Common.Interfaces.Persistance;

public interface ICurrencyRepository
{
    Task<List<Currency>> GetAllCurrencies();
    
    Task<Currency?> GetCurrencyById(CurrencyId id);
    
    Task<Currency?> GetCurrencyBySymbol(string symbol);
    
    void AddCurrency(Currency currency);
    
    void UpdateCurrency(Currency currency);

}