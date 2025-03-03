using ZBank.Domain.Common.Models;
using ZBank.Domain.CurrencyAggregate.ValueObjects;

namespace ZBank.Domain.CurrencyAggregate;

public class Currency : AggregateRoot<CurrencyId>
{
    public decimal Price { get; private set; }

    public decimal PriceChangeIn24Hours { get; private set; }

    private Currency(CurrencyId symbol, decimal price, decimal openPrice) : base(symbol) 
    {
        Id = symbol;
        Price = price;
        PriceChangeIn24Hours = Math.Round((price - openPrice) / openPrice * 100, 2);
    }

    public static Currency Create(string symbol, decimal price, decimal openPrice)
    {
        return new Currency(CurrencyId.Create(symbol), price, openPrice);
    }

    public void UpdatePrice(decimal price)
    {
        Price = price;
    }
    
#pragma warning disable CS8618
    private Currency()
#pragma warning restore CS8618
    {
    }
}