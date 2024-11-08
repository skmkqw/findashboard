using ZBank.Domain.Common.Models;

namespace ZBank.Domain.TeamAggregate.ValueObjects;

public class TeamId : ValueObject, IEntityId<TeamId, Guid>
{
    public Guid Value { get; }
    
    private TeamId(Guid value)
    {
        Value = value;
    }
    
    public static TeamId CreateUnique()
    {
        return new TeamId(Guid.NewGuid());
    }

    public static TeamId Create(Guid value)
    {
        return new TeamId(value);
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}