using ZBank.Application.Common.Interfaces.Persistance;
using ZBank.Domain.TeamAggregate;

namespace ZBank.Infrastructure.Persistance.Repositories;

public class TeamRepository : ITeamRepository
{
    private readonly ZBankDbContext _dbContext;

    public TeamRepository(ZBankDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void Add(Team team)
    {
        _dbContext.Teams.Add(team);
    }
}