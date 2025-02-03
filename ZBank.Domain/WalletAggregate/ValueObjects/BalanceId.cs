using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ZBank.Domain.Common.Attributes;
using ZBank.Domain.Common.Models;

namespace ZBank.Domain.WalletAggregate.ValueObjects;

[EfCoreValueConverter(typeof(BalanceIdValueConverter))]
public class BalanceId : ValueObject, IEntityId<BalanceId, Guid>
{
    public Guid Value { get; }
    
    private BalanceId(Guid value)
    {
        Value = value;
    }
    
    public static BalanceId CreateUnique()
    {
        return new BalanceId(Guid.NewGuid());
    }

    public static BalanceId Create(Guid value)
    {
        return new BalanceId(value);
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
    
    public class BalanceIdValueConverter : ValueConverter<BalanceId, Guid>
    {
        public BalanceIdValueConverter()
            : base(
                id => id.Value,
                value => Create(value)
            ) { }
    }
}