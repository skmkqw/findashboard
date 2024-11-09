using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ZBank.Domain.ActivityAggregate.ValueObjects;
using ZBank.Domain.Common.Attributes;
using ZBank.Domain.Common.Models;

namespace ZBank.Domain.TeamAggregate.ValueObjects;

[EfCoreValueConverter(typeof(TeamIdValueConverter))]
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
    
    public class TeamIdValueConverter : ValueConverter<TeamId, Guid>
    {
        public TeamIdValueConverter()
            : base(
                id => id.Value,
                value => Create(value)
            ) { }
    }
}