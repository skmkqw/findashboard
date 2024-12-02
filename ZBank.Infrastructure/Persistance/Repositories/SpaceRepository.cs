using ZBank.Application.Common.Interfaces.Persistance;
using ZBank.Domain.TeamAggregate;
using ZBank.Domain.TeamAggregate.ValueObjects;

namespace ZBank.Infrastructure.Persistance.Repositories;

public class SpaceRepository : ISpaceRepository
{
    private readonly ZBankDbContext _dbContext;

    public SpaceRepository(ZBankDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<PersonalSpace?> GetByIdAsync(TeamId id)
    {
        return await _dbContext.PersonalSpaces.FindAsync(id);
    }

    public void Add(PersonalSpace space)
    {
        _dbContext.PersonalSpaces.Add(space);
    }
}