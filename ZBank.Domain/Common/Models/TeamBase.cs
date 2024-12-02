using ZBank.Domain.ActivityAggregate.ValueObjects;
using ZBank.Domain.ProjectAggregate.ValueObjects;
using ZBank.Domain.TeamAggregate.ValueObjects;

namespace ZBank.Domain.Common.Models;

public abstract class TeamBase : AggregateRoot<TeamId>
{
    public string Name { get; protected set; }

    public string? Description { get; protected set; }

    public IReadOnlyList<ProjectId> ProjectIds => _projectIds.AsReadOnly();

    public IReadOnlyList<ActivityId> ActivityIds => _activityIds.AsReadOnly();

    protected readonly List<ProjectId> _projectIds = new();
    
    protected readonly List<ActivityId> _activityIds = new();

    protected TeamBase(TeamId id, string name, string? description) : base(id)
    {
        Id = id;
        Name = name;
        Description = description;
    }
    
    public abstract void AddProject(ProjectId projectId);

    public abstract void DeleteProject(ProjectId projectId);

    public abstract void AddActivity(ActivityId activityId);

    public abstract void DeleteActivity(ActivityId activityId);
    
#pragma warning disable CS8618
    protected TeamBase()
#pragma warning restore CS8618
    {
    }
}