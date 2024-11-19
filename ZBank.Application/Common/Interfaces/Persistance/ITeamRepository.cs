using ZBank.Domain.TeamAggregate;
using ZBank.Domain.TeamAggregate.ValueObjects;

namespace ZBank.Application.Common.Interfaces.Persistance;

public interface ITeamRepository
{
    Task<Team?> GetByIdAsync(TeamId id);
    void Add(Team team);
}