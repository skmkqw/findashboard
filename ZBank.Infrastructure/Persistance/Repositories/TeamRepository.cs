using ZBank.Application.Common.Interfaces.Persistance;
using ZBank.Application.Teams.Common;
using ZBank.Domain.Common.Models;
using ZBank.Domain.TeamAggregate;
using ZBank.Domain.TeamAggregate.ValueObjects;
using ZBank.Domain.UserAggregate;

namespace ZBank.Infrastructure.Persistance.Repositories;

public class TeamRepository : ITeamRepository
{
    private readonly ZBankDbContext _dbContext;

    public TeamRepository(ZBankDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Team?> GetTeamByIdAsync(TeamId teamId)
    {
        return await _dbContext.Teams.FindAsync(teamId);
    }
    
    public async Task<PersonalSpace?> GetSpaceByIdAsync(TeamId spaceId)
    {
        return await _dbContext.PersonalSpaces.FindAsync(spaceId);
    }

    public async Task<TeamValidationDetails?> GetTeamValidationDetailsAsync(TeamId teamId, User member)
    {
        TeamBase? teamOrSpace = await _dbContext.Teams.FindAsync(teamId) as TeamBase 
                                ?? await GetSpaceByIdAsync(teamId);

        return teamOrSpace is not null ? new TeamValidationDetails(teamOrSpace, member) : null;
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