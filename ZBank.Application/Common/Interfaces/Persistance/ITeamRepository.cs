using ZBank.Domain.TeamAggregate;

namespace ZBank.Application.Common.Interfaces.Persistance;

public interface ITeamRepository
{
    void Add(Team team);
}