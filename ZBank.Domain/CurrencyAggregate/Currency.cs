using ZBank.Domain.Common.Models;
using ZBank.Domain.CurrencyAggregate.ValueObjects;

namespace ZBank.Domain.CurrencyAggregate;

public class Currency : AggregateRoot<CurrencyId>
{
    public string Symbol { get; private set; }
    
    public decimal Price { get; private set; }

    private Currency(CurrencyId id, string symbol, decimal price) : base(id) 
    {
        Symbol = symbol;
        Price = price;
    }

    public static Currency Create(string symbol, decimal price)
    {
        return new Currency(CurrencyId.CreateUnique(), symbol, price);
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