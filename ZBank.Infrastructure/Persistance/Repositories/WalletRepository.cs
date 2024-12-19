using ZBank.Application.Common.Interfaces.Persistance;
using ZBank.Domain.WalletAggregate;

namespace ZBank.Infrastructure.Persistance.Repositories;

public class WalletRepository : IWalletRepository
{
    private readonly ZBankDbContext _dbContext;
    
    public WalletRepository(ZBankDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public void Add(Wallet wallet)
    {
        _dbContext.Wallets.Add(wallet);
    }
}