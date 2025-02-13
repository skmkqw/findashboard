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

    public async Task<Team?> GetTeamByIdAsync(TeamId id)
    {
        return await _dbContext.Teams.FindAsync(id);
    }
    
    public async Task<PersonalSpace?> GetByIdSpaceAsync(TeamId id)
    {
        return await _dbContext.PersonalSpaces.FindAsync(id);
    }

    public void AddTeam(Team team)
    {
        _dbContext.Teams.Add(team);
    }

    public void AddSpace(PersonalSpace space)
    {
        _dbContext.PersonalSpaces.Add(space);
    }
}