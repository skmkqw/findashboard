using ZBank.Domain.Common.Models;
using ZBank.Domain.WalletAggregate.ValueObjects;

namespace ZBank.Domain.WalletAggregate.Entities;

public class Balance : Entity<BalanceId>
{
    public string Symbol { get; set; }

    public decimal Amount { get; set; }

    private Balance(BalanceId id, string symbol, decimal amount) : base(id) 
    {
        Symbol = symbol;
        Amount = amount;
    }
    
    public static Balance Create(string symbol, decimal amount)
    {
        return new Balance(BalanceId.CreateUnique(), symbol, amount);
    }
}