using ZBank.Domain.ActivityAggregate.ValueObjects;
using ZBank.Domain.Common.Models;
using ZBank.Domain.ProfileAggregate.ValueObjects;
using ZBank.Domain.ProjectAggregate.ValueObjects;
using ZBank.Domain.TeamAggregate.ValueObjects;
using ZBank.Domain.UserAggregate.ValueObjects;

namespace ZBank.Domain.TeamAggregate;

public class Team : TeamBase
{ 
    private readonly List<UserId> _userIds;

    public IReadOnlyList<UserId> UserIds => _userIds.AsReadOnly();

    private Team(TeamId id, string name, string? description, List<UserId> userIds) 
        : base(id, name, description)
    {
        _userIds = userIds;
    }

    public static Team Create(string name, string? description, List<UserId> userIds)
    {
        return new Team(TeamId.CreateUnique(), name, description, userIds);
    }

    public override void AddProfile(ProfileId profileId) => _profileIds.Add(profileId);

    public override void DeleteProfile(ProfileId profileId) => _profileIds.Remove(profileId);

    public override void AddProject(ProjectId projectId) => _projectIds.Add(projectId);

    public override void DeleteProject(ProjectId projectId) => _projectIds.Remove(projectId);

    public override void AddActivity(ActivityId activityId) => _activityIds.Add(activityId);

    public override void DeleteActivity(ActivityId activityId) => _activityIds.Remove(activityId);

    public void AddUserId(UserId userId) => _userIds.Add(userId);

    public void DeleteUserId(UserId userId) => _userIds.Remove(userId);
    
#pragma warning disable CS8618
    private Team()
#pragma warning restore CS8618
    {
    }
}