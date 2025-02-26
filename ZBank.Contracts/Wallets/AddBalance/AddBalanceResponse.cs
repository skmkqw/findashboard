namespace ZBank.Contracts.Wallets.AddBalance;

public record AddBalanceResponse(Guid Id, string CurrencySymbol, decimal Amount);