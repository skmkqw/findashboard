using ZBank.Domain.Common.Models;
using ZBank.Domain.TeamAggregate;
using ZBank.Domain.TeamAggregate.ValueObjects;
using ZBank.Domain.UserAggregate;
using ZBank.Domain.UserAggregate.ValueObjects;

namespace ZBank.Application.Common.Models.Validation;

public record TeamValidationDetails
{
    private readonly TeamBase _teamOrSpace;
    
    private readonly User _member;

    public TeamValidationDetails(TeamBase teamOrSpace, User member)
    {
        _teamOrSpace = teamOrSpace;
        _member = member;
    }

    public bool IsTeam => _teamOrSpace is Team;
    
    public bool HasAccess => _teamOrSpace switch
    {
        PersonalSpace space => space.OwnerId == _member.Id,
        Team team => team.UserIds.Contains(_member.Id),
        _ => false
    };
    
    public TeamId TeamId => _teamOrSpace.Id;
    
    public UserId MemberId => _member.Id;
    
    public (TeamBase Team, User Member) GetEntities() => (_teamOrSpace, _member);
    
    public TeamBase GetTeamOrSpace() => _teamOrSpace;

    public bool MemberHasAccess(UserId memberId) => _teamOrSpace switch
    {
        PersonalSpace space => space.OwnerId == memberId,
        Team team => team.UserIds.Contains(memberId),
        _ => false
    };
}