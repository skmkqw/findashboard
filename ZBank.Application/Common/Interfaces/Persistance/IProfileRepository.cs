using ZBank.Domain.ProfileAggregate;
using ZBank.Domain.UserAggregate.ValueObjects;

namespace ZBank.Application.Common.Interfaces.Persistance;

public interface IProfileRepository
{
    Task<Profile?> GetByIdAsync(UserId id);
    
    void Add(Profile profile);
}