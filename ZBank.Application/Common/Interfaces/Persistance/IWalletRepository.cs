using ZBank.Domain.WalletAggregate;
using ZBank.Domain.WalletAggregate.ValueObjects;

namespace ZBank.Application.Common.Interfaces.Persistance;

public interface IWalletRepository
{
    Task<Wallet?> GetById(WalletId id);
    void Add(Wallet wallet);
}