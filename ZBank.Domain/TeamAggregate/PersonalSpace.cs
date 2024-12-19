using ZBank.Domain.ActivityAggregate.ValueObjects;
using ZBank.Domain.Common.Models;
using ZBank.Domain.ProfileAggregate.ValueObjects;
using ZBank.Domain.ProjectAggregate.ValueObjects;
using ZBank.Domain.TeamAggregate.ValueObjects;
using ZBank.Domain.UserAggregate.ValueObjects;

namespace ZBank.Domain.TeamAggregate ;

public class PersonalSpace : TeamBase
{
    public UserId OwnerId { get; init; }

    private PersonalSpace(TeamId id, string name, string? description, UserId ownerId)
        : base(id, name, description)
    {
        OwnerId = ownerId;
    }
    
    public static PersonalSpace Create(string name, string? description, UserId ownerId)
    {
        return new PersonalSpace(TeamId.CreateUnique(), name, description, ownerId);
    }
    
    public override void AddProfile(ProfileId profileId) => _profileIds.Add(profileId);

    public override void DeleteProfile(ProfileId profileId) => _profileIds.Remove(profileId);
    
    public override void AddProject(ProjectId projectId) => _projectIds.Add(projectId);

    public override void DeleteProject(ProjectId projectId) => _projectIds.Remove(projectId);

    public override void AddActivity(ActivityId activityId) => _activityIds.Add(activityId);

    public override void DeleteActivity(ActivityId activityId) => _activityIds.Remove(activityId);
}