using ZBank.Domain.CurrencyAggregate;
using ZBank.Domain.CurrencyAggregate.ValueObjects;
using ZBank.Domain.TeamAggregate.ValueObjects;

namespace ZBank.Application.Common.Interfaces.Persistance;

public interface ICurrencyRepository
{
    Task<List<Currency>> GetAllAsync();
    
    Task<Currency?> GetCurrencyBySymbolAsync(CurrencyId symbol);

    Task<List<Currency>> GetTeamCurrenciesAsync(TeamId teamId);
    
    Task ReplaceAllCurrenciesAsync(List<Currency> newCurrencies);
}