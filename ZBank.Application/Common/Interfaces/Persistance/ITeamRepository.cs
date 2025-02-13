using ZBank.Domain.TeamAggregate;
using ZBank.Domain.TeamAggregate.ValueObjects;

namespace ZBank.Application.Common.Interfaces.Persistance;

public interface ITeamRepository
{
    Task<Team?> GetTeamByIdAsync(TeamId id);
    
    Task<PersonalSpace?> GetByIdSpaceAsync(TeamId id);

    void AddTeam(Team team);
    
    void AddSpace(PersonalSpace space);
}