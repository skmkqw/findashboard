using Microsoft.EntityFrameworkCore;
using ZBank.Application.Common.Interfaces.Persistance;
using ZBank.Domain.CurrencyAggregate;
using ZBank.Domain.CurrencyAggregate.ValueObjects;

namespace ZBank.Infrastructure.Persistance.Repositories;

public class CurrencyRepository : ICurrencyRepository
{
    private readonly ZBankDbContext _dbContext;

    public CurrencyRepository(ZBankDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<List<Currency>> GetAllCurrencies()
    {
        return await _dbContext.Currencies.ToListAsync();
    }

    public async Task<Currency?> GetCurrencyById(CurrencyId id)
    {
        return await _dbContext.Currencies.FindAsync(id);
    }

    public async Task<Currency?> GetCurrencyBySymbol(string symbol)
    {
        return await _dbContext.Currencies.FirstOrDefaultAsync(x => x.Symbol == symbol);
    }

    public void AddCurrency(Currency currency)
    {
        _dbContext.Currencies.Add(currency);
    }

    public void UpdateCurrency(Currency currency)
    {
        _dbContext.Currencies.Update(currency);
    }
}