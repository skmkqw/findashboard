namespace ZBank.Contracts.Currencies;

public record GetCurrencyResponse(string Symbol, decimal Price, decimal PriceChangeIn24Hours);

public record GetCurrencyUpdatesResponse(List<GetCurrencyResponse> Currencies);