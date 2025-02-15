using ZBank.Application.Common.Models.Validation;
using ZBank.Domain.TeamAggregate;
using ZBank.Domain.TeamAggregate.ValueObjects;
using ZBank.Domain.UserAggregate;

namespace ZBank.Application.Common.Interfaces.Persistance;

public interface ITeamRepository
{
    Task<Team?> GetTeamByIdAsync(TeamId teamId);
    
    Task<PersonalSpace?> GetSpaceByIdAsync(TeamId spaceId);
    
    Task<TeamValidationDetails?> GetTeamValidationDetailsAsync(TeamId teamId, User member);

    void AddTeam(Team team);
    
    void AddSpace(PersonalSpace space);
}