using ZBank.Domain.Common.Models;
using ZBank.Domain.WalletAggregate.ValueObjects;

namespace ZBank.Domain.WalletAggregate;

public class Wallet : AggregateRoot<WalletId>
{
    public string Address { get; }
    
    
    //TODO This is probably not how i want to implement it
    public string Type { get; }

    private Wallet(string address, string type)
    {
        Address = address;
        Type = type;
    }

    public static Wallet Create(string address, string type)
    {
        return new Wallet(address, type);
    }
    
#pragma warning disable CS8618
    private Wallet()
#pragma warning restore CS8618
    {
    }
}