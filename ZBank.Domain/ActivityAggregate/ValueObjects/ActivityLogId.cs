using ZBank.Domain.Common.Models;

namespace ZBank.Domain.ActivityAggregate.ValueObjects;

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
}