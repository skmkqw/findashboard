using ZBank.Application.Common.Models.Validation;
using ZBank.Domain.ProfileAggregate;
using ZBank.Domain.ProfileAggregate.ValueObjects;

namespace ZBank.Application.Common.Interfaces.Persistance;

public interface IProfileRepository
{
    Task<Profile?> GetByIdAsync(ProfileId id);
    
    Task<ProfileValidationDetails?> GetProfileValidationDetailsAsync(ProfileId profileId);
    
    void Add(Profile profile);
}