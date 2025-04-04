using ZBank.Application.Common.Models.Validation;
using ZBank.Domain.TeamAggregate.ValueObjects;
using ZBank.Domain.WalletAggregate;
using ZBank.Domain.WalletAggregate.ValueObjects;

namespace ZBank.Application.Common.Interfaces.Persistance;

public interface IWalletRepository
{
    Task<List<Wallet>> GetAllAsync();
    
    Task<Wallet?> GetById(WalletId walletId);
    
    Task<List<Wallet>> GetTeamWalletsAsync(TeamId teamId);
    
    Task<WalletValidationDetails?> GetWalletValidationDetails(WalletId walletId);
    
    void Add(Wallet wallet);
}