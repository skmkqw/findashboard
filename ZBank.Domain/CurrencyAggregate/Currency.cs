using ZBank.Domain.Common.Models;
using ZBank.Domain.CurrencyAggregate.ValueObjects;

namespace ZBank.Domain.CurrencyAggregate;

public class Currency : AggregateRoot<CurrencyId>
{
    public decimal Price { get; private set; }

    private Currency(CurrencyId symbol, decimal price) : base(symbol) 
    {
        Id = symbol;
        Price = price;
    }

    public static Currency Create(string symbol, decimal price)
    {
        return new Currency(CurrencyId.Create(symbol), price);
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