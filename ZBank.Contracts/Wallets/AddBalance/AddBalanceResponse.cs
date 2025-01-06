namespace ZBank.Contracts.Wallets.AddBalance;

public record AddBalanceResponse(Guid Id, Guid CurrencyId, decimal Amount);