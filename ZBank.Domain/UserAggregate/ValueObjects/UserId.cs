using ZBank.Domain.Common.Models;

namespace ZBank.Domain.UserAggregate.ValueObjects;

public class UserId : ValueObject
{
    public Guid Value { get; }

    private UserId(Guid value)
    {
        Value = value;
    }
    
    public static UserId CreateUnique()
    {
        return new UserId(Guid.NewGuid());
    }

    public static UserId Create(Guid value)
    {
        return new UserId(value);
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}