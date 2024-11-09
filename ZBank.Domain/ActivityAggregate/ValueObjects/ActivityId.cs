using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ZBank.Domain.Common.Attributes;
using ZBank.Domain.Common.Models;

namespace ZBank.Domain.ActivityAggregate.ValueObjects;

[EfCoreValueConverter(typeof(ActivityIdValueConverter))]
public class ActivityId : ValueObject, IEntityId<ActivityId, Guid>
{
    public Guid Value { get; }

    private ActivityId(Guid value)
    {
        Value = value;
    }
    
    public static ActivityId CreateUnique()
    {
        return new ActivityId(Guid.NewGuid());
    }

    public static ActivityId Create(Guid value)
    {
        return new ActivityId(value);
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
    
    public class ActivityIdValueConverter : ValueConverter<ActivityId, Guid>
    {
        public ActivityIdValueConverter()
            : base(
                id => id.Value,
                value => Create(value)
            ) { }
    }
}