using ZBank.Domain.Common.Models;
using ZBank.Domain.ProfileAggregate.ValueObjects;
using ZBank.Domain.WalletAggregate.ValueObjects;

namespace ZBank.Domain.WalletAggregate;

public class Wallet : AggregateRoot<WalletId>
{
    public string Address { get; private set; }
    
    public WalletType Type { get; private set; }

    public ProfileId ProfileId { get; }

    private Wallet(string address, WalletType type, ProfileId profileId)
    {
        Address = address;
        Type = type;
        ProfileId = profileId;
    }

    public static Wallet Create(string address, WalletType type, ProfileId profileId)
    {
        return new Wallet(address, type, profileId);
    }

    public void Update(string address, WalletType type)
    {
        Address = address;
        Type = type;
    }
    
#pragma warning disable CS8618
    private Wallet()
#pragma warning restore CS8618
    {
    }
}