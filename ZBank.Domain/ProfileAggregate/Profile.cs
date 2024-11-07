using ZBank.Domain.Common.Models;
using ZBank.Domain.ProfileAggregate.ValueObjects;
using ZBank.Domain.UserAggregate.ValueObjects;
using ZBank.Domain.WalletAggregate;

namespace ZBank.Domain.ProfileAggregate;

public class Profile : AggregateRoot<ProfileId>
{
    public string Name { get; }
    
    public UserId OwnerId { get; }
    
    public IReadOnlyList<Wallet> Wallets => _wallets;
    
    private readonly List<Wallet> _wallets = new();

    private Profile(string name, UserId ownerId)
    {
        Name = name;
        OwnerId = ownerId;
    }

    public static Profile Create(string name, UserId ownerId)
    {
        return new Profile(name, ownerId);
    }
    
#pragma warning disable CS8618
    private Profile()
#pragma warning restore CS8618
    {
    }
}