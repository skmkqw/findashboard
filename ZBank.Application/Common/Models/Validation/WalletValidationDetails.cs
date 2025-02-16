using ZBank.Domain.ProfileAggregate;
using ZBank.Domain.ProfileAggregate.ValueObjects;
using ZBank.Domain.UserAggregate.ValueObjects;
using ZBank.Domain.WalletAggregate;
using ZBank.Domain.WalletAggregate.ValueObjects;

namespace ZBank.Application.Common.Models.Validation;

public class WalletValidationDetails
{
    private readonly Wallet _wallet;
    
    private readonly Profile _profile;

    public WalletValidationDetails(Wallet wallet, Profile profile)
    {
        _wallet = wallet;
        _profile = profile;
    }
    
    public WalletId WalletId => _wallet.Id;
    
    public ProfileId ProfileId => _profile.Id;
    
    public UserId OwnerId => _profile.OwnerId;
    
    public bool HasAccess(UserId userId) => _wallet.ProfileId == _profile.Id && _profile.OwnerId == userId;
    
    public (Wallet Wallet, Profile Profile) GetEntities() => (_wallet, _profile);

    public Wallet GetWallet() => _wallet;
}