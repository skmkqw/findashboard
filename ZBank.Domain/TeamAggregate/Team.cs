using ZBank.Domain.Common.Models;
using ZBank.Domain.ProjectAggregate.ValueObjects;
using ZBank.Domain.TeamAggregate.ValueObjects;
using ZBank.Domain.UserAggregate.ValueObjects;

namespace ZBank.Domain.TeamAggregate;

public class Team : AggregateRoot<TeamId>
{ 
    public string Name { get; }
    
    public IReadOnlyList<UserId> UserIds => _userIds.AsReadOnly();
    
    public IReadOnlyList<ProjectId> ProjectIds => _projectIds.AsReadOnly();
    
    private readonly List<UserId> _userIds;
    
    private readonly List<ProjectId> _projectIds = new();

    private Team(string name, List<UserId> userIds)
    {
        Id = TeamId.CreateUnique();
        Name = name;
        _userIds = userIds;
    }
    
#pragma warning disable CS8618
    private Team()
#pragma warning restore CS8618
    {
    }
}