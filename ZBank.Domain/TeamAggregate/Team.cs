using ZBank.Domain.ActivityAggregate.ValueObjects;
using ZBank.Domain.Common.Models;
using ZBank.Domain.ProjectAggregate.ValueObjects;
using ZBank.Domain.TeamAggregate.ValueObjects;
using ZBank.Domain.UserAggregate.ValueObjects;

namespace ZBank.Domain.TeamAggregate;

public class Team : AggregateRoot<TeamId>
{ 
    public string Name { get; private set;  }

    public string Description { get; private set; }
    
    public IReadOnlyList<UserId> UserIds => _userIds.AsReadOnly();
    
    public IReadOnlyList<ProjectId> ProjectIds => _projectIds.AsReadOnly();
    
    public IReadOnlyList<ActivityId> ActivityIds => _activityIds.AsReadOnly();
    
    private readonly List<UserId> _userIds;
    
    private readonly List<ProjectId> _projectIds = new();
    
    private readonly List<ActivityId> _activityIds = new();

    private Team(TeamId id, string name, string description, List<UserId> userIds) : base(id)
    {
        Id = TeamId.CreateUnique();
        Name = name;
        Description = description;
        _userIds = userIds;
    }

    public static Team Create(string name, string description, List<UserId> userIds)
    {
        return new Team(TeamId.CreateUnique(), name, description, userIds);
    }

    public void Update(string name, string description)
    {
        Name = name;
        Description = description;
    }

    public void AddUser(UserId userId) => _userIds.Add(userId);

    public void DeleteUser(UserId userId) => _userIds.Remove(userId);

    public void AddProject(ProjectId projectId) => _projectIds.Add(projectId);

    public void DeleteProject(ProjectId projectId) => _projectIds.Remove(projectId);

    public void AddActivity(ActivityId activityId) => _activityIds.Add(activityId);

    public void DeleteActivity(ActivityId activityId) => _activityIds.Remove(activityId);
    
#pragma warning disable CS8618
    private Team()
#pragma warning restore CS8618
    {
    }
}