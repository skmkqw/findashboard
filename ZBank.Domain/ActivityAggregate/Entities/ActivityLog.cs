using ZBank.Domain.ActivityAggregate.ValueObjects;
using ZBank.Domain.Common.Models;
using ZBank.Domain.ProfileAggregate.ValueObjects;
using ZBank.Domain.UserAggregate.ValueObjects;
using ZBank.Domain.WalletAggregate.ValueObjects;

namespace ZBank.Domain.ActivityAggregate.Entities;

public class ActivityLog : Entity<ActivityLogId>
{
    public UserId UserId { get; }

    public ProfileId ProfileId { get; }

    public WalletId WalletId { get; }

    public DateTime TimeStamp { get; }
    
    private ActivityLog(UserId userId, ProfileId profileId, WalletId walletId)
    {
        UserId = userId;
        ProfileId = profileId;
        WalletId = walletId;
        TimeStamp = DateTime.UtcNow;
    }

    public static ActivityLog Create(UserId userId, ProfileId profileId, WalletId walletId)
    {
        return new ActivityLog(userId, profileId, walletId);
    }
    
#pragma warning disable CS8618
    private ActivityLog()
#pragma warning restore CS8618
    {
    }
}