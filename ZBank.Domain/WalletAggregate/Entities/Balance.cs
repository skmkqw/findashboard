using ZBank.Domain.Common.Models;
using ZBank.Domain.CurrencyAggregate.ValueObjects;
using ZBank.Domain.WalletAggregate.ValueObjects;

namespace ZBank.Domain.WalletAggregate.Entities;

public class Balance : Entity<BalanceId>
{
    public CurrencyId CurrencyId { get; init; }

    public decimal Amount { get; private set; }

    private Balance(BalanceId id, CurrencyId currencyId, decimal amount) : base(id) 
    {
        CurrencyId = currencyId;
        Amount = amount;
    }
    
    public static Balance Create(CurrencyId symbol, decimal amount)
    {
        return new Balance(BalanceId.CreateUnique(), symbol, amount);
    }

    public void ChangeAmount(decimal amount)
    {
        Amount = amount;
    }
}