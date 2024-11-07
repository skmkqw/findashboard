using ZBank.Domain.Common.Models;
using ZBank.Domain.ProfileAggregate.ValueObjects;
using ZBank.Domain.UserAggregate.ValueObjects;
using ZBank.Domain.WalletAggregate.ValueObjects;

namespace ZBank.Domain.ProfileAggregate;

public class Profile : AggregateRoot<ProfileId>
{
    public string Name { get; private set;  }
    
    public UserId OwnerId { get; }
    
    public IReadOnlyList<WalletId> WalletIds => _walletIds;
    
    private readonly List<WalletId> _walletIds = new();

    private Profile(string name, UserId ownerId)
    {
        Name = name;
        OwnerId = ownerId;
    }

    public static Profile Create(string name, UserId ownerId)
    {
        return new Profile(name, ownerId);
    }
    
    public void Update(string name)
    {
        Name = name;
    }

    public void AddWallet(WalletId walletId)
    {
        _walletIds.Add(walletId);
    }

    public void DeleteWallet(WalletId walletId)
    {
        _walletIds.Remove(walletId);
    }
#pragma warning disable CS8618
    private Profile()
#pragma warning restore CS8618
    {
    }
}