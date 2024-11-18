using ZBank.Application.Common.Interfaces.Persistance;
using ZBank.Domain.TeamAggregate;
using ZBank.Domain.TeamAggregate.ValueObjects;

namespace ZBank.Infrastructure.Persistance.Repositories;

public class TeamRepository : ITeamRepository
{
    private readonly ZBankDbContext _dbContext;

    public TeamRepository(ZBankDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Team?> GetByIdAsync(TeamId id)
    {
        return await _dbContext.Teams.FindAsync(id);
    }

    public void Add(Team team)
    {
        _dbContext.Teams.Add(team);
    }
}