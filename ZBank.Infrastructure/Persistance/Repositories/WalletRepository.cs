using Microsoft.EntityFrameworkCore;
using ZBank.Application.Common.Interfaces.Persistance;
using ZBank.Application.Common.Models.Validation;
using ZBank.Domain.TeamAggregate.ValueObjects;
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

    public async Task<List<Wallet>> GetAllAsync()
    {
        return await _dbContext.Wallets.ToListAsync(); 
    }

    public async Task<Wallet?> GetById(WalletId walletId)
    {
        return await _dbContext.Wallets.FindAsync(walletId);
    }

    public async Task<List<Wallet>> GetTeamWalletsAsync(TeamId teamId)
    {
        return await _dbContext.Wallets
            .FromSqlRaw(@"
                SELECT DISTINCT w.*
                FROM ""Wallets"" w
                INNER JOIN ""Profiles"" p ON w.""ProfileId"" = p.""Id""
                INNER JOIN ""Teams"" t ON p.""TeamId"" = t.""Id""
                WHERE t.""Id"" = {0}", teamId.Value)
            .ToListAsync();
    }

    public async Task<WalletValidationDetails?> GetWalletValidationDetails(WalletId walletId)
    {
        var query = _dbContext.Wallets
            .Where(wallet => wallet.Id == walletId)
            .Join(
                _dbContext.Profiles,
                wallet => wallet.ProfileId,
                profile => profile.Id,
                (wallet, user) => new WalletValidationDetails(wallet, user)
            );

        return await query.FirstOrDefaultAsync();
    }

    public void Add(Wallet wallet)
    {
        _dbContext.Wallets.Add(wallet);
    }
}