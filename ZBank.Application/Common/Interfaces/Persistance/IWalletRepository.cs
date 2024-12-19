using ZBank.Domain.WalletAggregate;

namespace ZBank.Application.Common.Interfaces.Persistance;

public interface IWalletRepository
{
    void Add(Wallet wallet);
}