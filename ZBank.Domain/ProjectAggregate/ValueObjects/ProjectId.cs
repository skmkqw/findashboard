using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ZBank.Domain.Common.Attributes;
using ZBank.Domain.Common.Models;

namespace ZBank.Domain.ProjectAggregate.ValueObjects;

[EfCoreValueConverter(typeof(ProjectIdValueConverter))]
public class ProjectId : ValueObject, IEntityId<ProjectId, Guid>
{
    public Guid Value { get; }

    private ProjectId(Guid value)
    {
        Value = value;
    }
    
    public static ProjectId CreateUnique()
    {
        return new ProjectId(Guid.NewGuid());
    }

    public static ProjectId Create(Guid value)
    {
        return new ProjectId(value);
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
    
    public class ProjectIdValueConverter : ValueConverter<ProjectId, Guid>
    {
        public ProjectIdValueConverter()
            : base(
                id => id.Value,
                value => Create(value)
            ) { }
    }
}