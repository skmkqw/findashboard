using Microsoft.EntityFrameworkCore;
using ZBank.Application.Common.Interfaces.Persistance;
using ZBank.Application.Common.Models.Validation;
using ZBank.Domain.ProfileAggregate;
using ZBank.Domain.ProfileAggregate.ValueObjects;

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

    public async Task<ProfileValidationDetails?> GetProfileWithOwnerAsync(ProfileId profileId)
    {
        var query = _dbContext.Profiles
            .Where(profile => profile.Id == profileId)
            .Join(
                _dbContext.Users,
                profile => profile.OwnerId,
                user => user.Id,
                (profile, user) => new ProfileValidationDetails(profile, user)
            );

        return await query.FirstOrDefaultAsync();
    }

    public void Add(Profile profile)
    {
        _dbContext.Profiles.Add(profile);
    }
}