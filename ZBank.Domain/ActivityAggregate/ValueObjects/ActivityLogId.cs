using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ZBank.Domain.Common.Attributes;
using ZBank.Domain.Common.Models;

namespace ZBank.Domain.ActivityAggregate.ValueObjects;

[EfCoreValueConverter(typeof(ActivityLogIdValueConverter))]
public class ActivityLogId : ValueObject, IEntityId<ActivityLogId, Guid>
{
    public Guid Value { get; }

    private ActivityLogId(Guid value)
    {
        Value = value;
    }
    
    public static ActivityLogId CreateUnique()
    {
        return new ActivityLogId(Guid.NewGuid());
    }

    public static ActivityLogId Create(Guid value)
    {
        return new ActivityLogId(value);
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
    
    public class ActivityLogIdValueConverter : ValueConverter<ActivityLogId, Guid>
    {
        public ActivityLogIdValueConverter()
            : base(
                id => id.Value,
                value => Create(value)
            ) { }
    }
}