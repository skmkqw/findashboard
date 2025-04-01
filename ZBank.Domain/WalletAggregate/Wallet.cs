using ZBank.Domain.Common.Models;
using ZBank.Domain.ProfileAggregate.ValueObjects;
using ZBank.Domain.WalletAggregate.Entities;
using ZBank.Domain.WalletAggregate.ValueObjects;

namespace ZBank.Domain.WalletAggregate;

public class Wallet : AggregateRoot<WalletId>
{
    public string Address { get; private set; }

    public WalletType Type { get; private set; } = WalletType.EVM;

    public ProfileId ProfileId { get; }

    public decimal TotalInUsd { get; private set; }

    public IReadOnlyList<Balance> Balances => _balances.AsReadOnly();

    private readonly List<Balance> _balances = new();
    
    private Wallet(WalletId id, string address, WalletType type, ProfileId profileId) : base(id)
    {
        Address = address;
        Type = type;
        ProfileId = profileId;
        TotalInUsd = 0;
    }

    public static Wallet Create(string address, WalletType type, ProfileId profileId)
    {
        return new Wallet(WalletId.CreateUnique(), address, type, profileId);
    }

    public void Update(string address, WalletType type)
    {
        Address = address;
        Type = type;
    }
    
    public void UpdateTotal() => TotalInUsd = _balances.Sum(x => x.TotalInUsd);
    
    public void AddBalance(Balance balance) => _balances.Add(balance);
    
    public void DeleteBalance(Balance balance) => _balances.Remove(balance);
    
#pragma warning disable CS8618
    private Wallet()
#pragma warning restore CS8618
    {
    }
}