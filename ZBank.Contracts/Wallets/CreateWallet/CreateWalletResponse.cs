namespace ZBank.Contracts.Wallets.CreateWallet;

public record CreateWalletResponse(Guid Id, string Address, string Type, Guid ProfileId);