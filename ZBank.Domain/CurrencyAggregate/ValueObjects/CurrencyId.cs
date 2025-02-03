using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ZBank.Domain.Common.Attributes;
using ZBank.Domain.Common.Models;

namespace ZBank.Domain.CurrencyAggregate.ValueObjects;

[EfCoreValueConverter(typeof(CurrencyIdValueConverter))]
public class CurrencyId : ValueObject, IEntityId<CurrencyId, Guid>
{
    public Guid Value { get; }
    
    private CurrencyId(Guid value)
    {
        Value = value;
    }
    
    public static CurrencyId CreateUnique()
    {
        return new CurrencyId(Guid.NewGuid());
    }

    public static CurrencyId Create(Guid value)
    {
        return new CurrencyId(value);
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
    
    public class CurrencyIdValueConverter : ValueConverter<CurrencyId, Guid>
    {
        public CurrencyIdValueConverter()
            : base(
                id => id.Value,
                value => Create(value)
            ) { }
    }
}