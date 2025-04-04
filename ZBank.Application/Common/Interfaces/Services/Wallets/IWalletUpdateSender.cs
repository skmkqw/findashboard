using ZBank.Domain.WalletAggregate;

namespace ZBank.Application.Common.Interfaces.Services.Wallets;

public interface IWalletUpdateSender
{
    Task SendUpdateAsync(List<Wallet> wallet, string groupName);
}