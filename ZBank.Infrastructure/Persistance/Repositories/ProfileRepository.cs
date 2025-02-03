using ZBank.Application.Common.Interfaces.Persistance;
using ZBank.Domain.ProfileAggregate;
using ZBank.Domain.ProfileAggregate.ValueObjects;
using ZBank.Domain.UserAggregate.ValueObjects;

namespace ZBank.Infrastructure.Persistance.Repositories;

public class ProfileRepository : IProfileRepository
{
    private readonly ZBankDbContext _dbContext;
    
    public ProfileRepository(ZBankDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<Profile?> GetByIdAsync(ProfileId id)
    {
        return await _dbContext.Profiles.FindAsync(id); 
    }

    public void Add(Profile profile)
    {
        _dbContext.Profiles.Add(profile);
    }
}