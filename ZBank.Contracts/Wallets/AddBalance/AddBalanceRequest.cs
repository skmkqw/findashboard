namespace ZBank.Contracts.Wallets.AddBalance;

public record AddBalanceRequest(Guid WalletId, string Symbol, decimal Amount);