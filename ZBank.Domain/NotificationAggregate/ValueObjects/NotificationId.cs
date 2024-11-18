using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ZBank.Domain.Common.Attributes;
using ZBank.Domain.Common.Models;

namespace ZBank.Domain.NotificationAggregate.ValueObjects;

[EfCoreValueConverter(typeof(NotificationIdValueConverter))]
public class NotificationId : ValueObject, IEntityId<NotificationId, Guid>
{
    public Guid Value { get; }

    private NotificationId(Guid value)
    {
        Value = value;
    }
    
    public static NotificationId CreateUnique()
    {
        return new NotificationId(Guid.NewGuid());
    }

    public static NotificationId Create(Guid value)
    {
        return new NotificationId(value);
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
    
    public class NotificationIdValueConverter : ValueConverter<NotificationId, Guid>
    {
        public NotificationIdValueConverter()
            : base(
                id => id.Value,
                value => Create(value)
            ) { }
    }
}