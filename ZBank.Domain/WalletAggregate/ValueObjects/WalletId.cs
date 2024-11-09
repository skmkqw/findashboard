using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ZBank.Domain.Common.Attributes;
using ZBank.Domain.Common.Models;

namespace ZBank.Domain.WalletAggregate.ValueObjects;

[EfCoreValueConverter(typeof(WalletIdValueConverter))]
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
    
    public class WalletIdValueConverter : ValueConverter<WalletId, Guid>
    {
        public WalletIdValueConverter()
            : base(
                id => id.Value,
                value => Create(value)
            ) { }
    }
}