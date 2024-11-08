using ZBank.Domain.Common.Models;

namespace ZBank.Domain.ProfileAggregate.ValueObjects;

public class ProfileId : ValueObject, IEntityId<ProfileId, Guid>
{
    public Guid Value { get; }

    private ProfileId(Guid value)
    {
        Value = value;
    }
    
    public static ProfileId CreateUnique()
    {
        return new ProfileId(Guid.NewGuid());
    }

    public static ProfileId Create(Guid value)
    {
        return new ProfileId(value);
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}