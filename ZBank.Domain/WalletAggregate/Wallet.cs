using ZBank.Domain.Common.Models;
using ZBank.Domain.ProfileAggregate.ValueObjects;
using ZBank.Domain.WalletAggregate.ValueObjects;

namespace ZBank.Domain.WalletAggregate;

public class Wallet : AggregateRoot<WalletId>
{
    public string Address { get; private set; }
    
    //TODO This is probably not how i want to implement it
    public string Type { get; private set; }

    public ProfileId ProfileId { get; }

    private Wallet(string address, string type, ProfileId profileId)
    {
        Address = address;
        Type = type;
        ProfileId = profileId;
    }

    public static Wallet Create(string address, string type, ProfileId profileId)
    {
        return new Wallet(address, type, profileId);
    }

    public void Update(string address, string type)
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