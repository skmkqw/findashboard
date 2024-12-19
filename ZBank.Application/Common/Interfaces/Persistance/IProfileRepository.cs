using ZBank.Domain.ProfileAggregate;
using ZBank.Domain.ProfileAggregate.ValueObjects;
using ZBank.Domain.UserAggregate.ValueObjects;

namespace ZBank.Application.Common.Interfaces.Persistance;

public interface IProfileRepository
{
    Task<Profile?> GetByIdAsync(ProfileId id);
    
    void Add(Profile profile);
}