using ZBank.Domain.ProfileAggregate;
using ZBank.Domain.ProfileAggregate.ValueObjects;
using ZBank.Domain.TeamAggregate.ValueObjects;
using ZBank.Domain.UserAggregate;
using ZBank.Domain.UserAggregate.ValueObjects;

namespace ZBank.Application.Common.Models.Validation;

public class ProfileValidationDetails
{
    private readonly Profile _profile;
    
    private readonly User _owner;

    public ProfileValidationDetails(Profile profile, User owner)
    {
        _profile = profile;
        _owner = owner;
    }
    
    public bool HasAccess => _profile.OwnerId == _owner.Id;
    
    public TeamId TeamId => _profile.TeamId;
    
    public ProfileId ProfileId => _profile.Id;
    
    public UserId OwnerId => _owner.Id;
    
    public (Profile profile, User Owner) GetEntities() => (_profile, _owner);
}