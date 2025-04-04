using ZBank.Domain.Common.Models;
using ZBank.Domain.CurrencyAggregate.ValueObjects;
using ZBank.Domain.WalletAggregate.ValueObjects;

namespace ZBank.Domain.WalletAggregate.Entities;

public class Balance : Entity<BalanceId>
{
    public CurrencyId CurrencyId { get; }

    public decimal Amount { get; private set; }
    
    public decimal TotalInUsd { get; private set; }

    private Balance(BalanceId id, CurrencyId currencyId, decimal amount) : base(id) 
    {
        CurrencyId = currencyId;
        Amount = amount;
        TotalInUsd = 0;
    }
    
    public static Balance Create(CurrencyId symbol, decimal amount)
    {
        return new Balance(BalanceId.CreateUnique(), symbol, amount);
    }

    public void UpdateAmount(decimal amount) => Amount = amount;
    
    public void UpdateTotal(decimal totalInUsd) => TotalInUsd = totalInUsd;
}