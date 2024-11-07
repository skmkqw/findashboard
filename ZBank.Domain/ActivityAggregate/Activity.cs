using ZBank.Domain.ActivityAggregate.ValueObjects;
using ZBank.Domain.Common.Models;
using ZBank.Domain.ProjectAggregate.ValueObjects;
using ZBank.Domain.TeamAggregate.ValueObjects;

namespace ZBank.Domain.ActivityAggregate;

public class Activity : AggregateRoot<ActivityId>
{
    public string Name { get; }

    public string Description { get; }

    public TeamId TeamId { get; }

    public ProjectId ProjectId { get; }

    private Activity(string name, string description, TeamId teamId, ProjectId projectId)
    {
        Name = name;
        Description = description;
        TeamId = teamId;
        ProjectId = projectId;
    }

    public static Activity Create(string name, string description, TeamId teamId, ProjectId projectId)
    {
        return new Activity(name, description, teamId, projectId);
    }
    
#pragma warning disable CS8618
    private Activity()
#pragma warning restore CS8618
    {
    }
}