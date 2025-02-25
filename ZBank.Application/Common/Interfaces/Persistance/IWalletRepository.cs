using ZBank.Application.Common.Models.Validation;
using ZBank.Domain.WalletAggregate;
using ZBank.Domain.WalletAggregate.ValueObjects;

namespace ZBank.Application.Common.Interfaces.Persistance;

public interface IWalletRepository
{
    Task<Wallet?> GetById(WalletId walletId);
    
    Task<WalletValidationDetails?> GetWalletValidationDetails(WalletId walletId);
    
    void Add(Wallet wallet);
}