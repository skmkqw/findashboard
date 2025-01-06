using ZBank.Application.Common.Interfaces.Persistance;
using ZBank.Domain.WalletAggregate;
using ZBank.Domain.WalletAggregate.ValueObjects;

namespace ZBank.Infrastructure.Persistance.Repositories;

public class WalletRepository : IWalletRepository
{
    private readonly ZBankDbContext _dbContext;
    
    public WalletRepository(ZBankDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Wallet?> GetById(WalletId id)
    {
        return await _dbContext.Wallets.FindAsync(id);
    }

    public void Add(Wallet wallet)
    {
        _dbContext.Wallets.Add(wallet);
    }
}