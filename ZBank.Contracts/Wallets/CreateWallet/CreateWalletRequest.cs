namespace ZBank.Contracts.Wallets.CreateWallet;

public record CreateWalletRequest(string Address, string Type, Guid ProfileId);