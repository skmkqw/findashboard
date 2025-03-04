using ZBank.Domain.Common.Models;
using ZBank.Domain.ProfileAggregate.ValueObjects;
using ZBank.Domain.TeamAggregate.ValueObjects;
using ZBank.Domain.UserAggregate.ValueObjects;
using ZBank.Domain.WalletAggregate.ValueObjects;

namespace ZBank.Domain.ProfileAggregate;

public class Profile : AggregateRoot<ProfileId>
{
    public string Name { get; private set; }
    
    public TeamId TeamId { get; }
    
    public UserId OwnerId { get; }
    
    public IReadOnlyList<WalletId> WalletIds => _walletIds;
    
    private readonly List<WalletId> _walletIds = new();

    private Profile(ProfileId id, string name, UserId ownerId, TeamId teamId) : base(id)
    {
        Name = name;
        OwnerId = ownerId;
        TeamId = teamId;
    }

    public static Profile Create(string name, UserId ownerId, TeamId teamId)
    {
        return new Profile(ProfileId.CreateUnique(), name, ownerId, teamId);
    }
    
    public void Update(string name)
    {
        Name = name;
    }
    
    public bool CanBeModifiedBy(UserId userId) => OwnerId == userId;

    public void AddWallet(WalletId walletId) => _walletIds.Add(walletId);

    public void DeleteWallet(WalletId walletId) => _walletIds.Remove(walletId);
    
#pragma warning disable CS8618
    private Profile()
#pragma warning restore CS8618
    {
    }
}