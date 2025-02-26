using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ZBank.Domain.Common.Attributes;
using ZBank.Domain.Common.Models;

namespace ZBank.Domain.CurrencyAggregate.ValueObjects;

[EfCoreValueConverter(typeof(CurrencyIdValueConverter))]
public class CurrencyId : ValueObject, IEntityId<CurrencyId, string>
{
    public string Value { get; }
    
    private CurrencyId(string value)
    {
        Value = value;
    }

    public static CurrencyId Create(string value)
    {
        return new CurrencyId(value);
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
    
    public class CurrencyIdValueConverter : ValueConverter<CurrencyId, string>
    {
        public CurrencyIdValueConverter()
            : base(
                id => id.Value,
                value => Create(value)
            ) { }
    }
}