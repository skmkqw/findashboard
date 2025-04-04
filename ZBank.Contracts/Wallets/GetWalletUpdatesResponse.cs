namespace ZBank.Contracts.Wallets;

public record GetBalanceResponse(Guid Id, string CurrencySymbol, decimal Amount, decimal TotalInUsd);

public record GetWalletUpdateResponse(Guid Id, List<GetBalanceResponse> Balances, decimal TotalInUsd);

public record GetWalletUpdatesResponse(List<GetWalletUpdateResponse> WalletUpdates);