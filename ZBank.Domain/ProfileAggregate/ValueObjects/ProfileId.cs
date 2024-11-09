using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ZBank.Domain.Common.Attributes;
using ZBank.Domain.Common.Models;

namespace ZBank.Domain.ProfileAggregate.ValueObjects;

[EfCoreValueConverter(typeof(ProfileIdValueConverter))]
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
    
    public class ProfileIdValueConverter : ValueConverter<ProfileId, Guid>
    {
        public ProfileIdValueConverter()
            : base(
                id => id.Value,
                value => Create(value)
            ) { }
    }
}