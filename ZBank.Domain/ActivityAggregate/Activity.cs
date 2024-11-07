using ZBank.Domain.ActivityAggregate.Entities;
using ZBank.Domain.ActivityAggregate.ValueObjects;
using ZBank.Domain.Common.Models;
using ZBank.Domain.ProjectAggregate.ValueObjects;
using ZBank.Domain.TeamAggregate.ValueObjects;

namespace ZBank.Domain.ActivityAggregate;

public class Activity : AggregateRoot<ActivityId>
{
    public string Name { get; private set; }

    public string Description { get; private set; }

    public TeamId TeamId { get; }

    public ProjectId ProjectId { get; private set; }
    
    public IReadOnlyList<ActivityLog> ActivityLogs => _activityLogs.AsReadOnly();
    
    private readonly List<ActivityLog> _activityLogs = new();

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

    public void Update(string name, string description, ProjectId projectId)
    {
        Name = name;
        Description = description;
        ProjectId = projectId;
    }

    public void AddLog(ActivityLog activityLog)
    {
        _activityLogs.Add(activityLog);
    }

    public void DeleteLog(ActivityLog activityLog)
    {
        _activityLogs.Remove(activityLog);
    }
    
#pragma warning disable CS8618
    private Activity()
#pragma warning restore CS8618
    {
    }
}