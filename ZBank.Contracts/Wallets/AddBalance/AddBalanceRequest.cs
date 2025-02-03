namespace ZBank.Contracts.Wallets.AddBalance;

public record AddBalanceRequest(Guid WalletId, Guid CurrencyId, decimal Amount);