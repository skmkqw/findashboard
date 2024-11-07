using ZBank.Domain.Common.Models;

namespace ZBank.Domain.WalletAggregate.ValueObjects;

public class WalletId : ValueObject, IEntityId<WalletId, Guid>
{
    public Guid Value { get; }
    
    private WalletId(Guid value)
    {
        Value = value;
    }
    
    public static WalletId CreateUnique()
    {
        return new WalletId(Guid.NewGuid());
    }

    public static WalletId Create(Guid value)
    {
        return new WalletId(value);
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}