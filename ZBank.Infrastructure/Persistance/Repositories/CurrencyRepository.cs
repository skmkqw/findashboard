using EFCore.BulkExtensions;
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

    public async Task<Currency?> GetCurrencyBySymbolAsync(CurrencyId symbol)
    {
        return await _dbContext.Currencies.FindAsync(symbol);
    }

    public async Task<List<Currency>> GetAllCurrenciesAsync()
    {
        return await _dbContext.Currencies.ToListAsync();
    }

    public async Task ReplaceAllCurrenciesAsync(List<Currency> newCurrencies)
    {
        await using var transaction = await _dbContext.Database.BeginTransactionAsync();

        await _dbContext.Currencies
            .ExecuteDeleteAsync();

        await _dbContext.BulkInsertAsync(newCurrencies, options => 
        {
            options.BatchSize = 500;
            options.IncludeGraph = true;
        });

        await transaction.CommitAsync();
    }
}