using ZBank.Domain.Common.Models;
using ZBank.Domain.NotificationAggregate.ValueObjects;
using ZBank.Domain.TeamAggregate.ValueObjects;
using ZBank.Domain.UserAggregate.ValueObjects;

namespace ZBank.Domain.NotificationAggregate;

public class TeamInviteNotification : Notification
{
    public TeamId TeamId { get; }

    public string TeamName { get; init; }
    
    internal TeamInviteNotification(NotificationId id,
        NotificationSender notificationSender,
        UserId receiverId,
        TeamId teamId, string teamName) : base(id, notificationSender, receiverId)
    {
        TeamId = teamId;
        TeamName = teamName;
        Content = $"{notificationSender.SenderFullName} has invited you to team: {teamName}";
    }
    
#pragma warning disable CS8618
    private TeamInviteNotification()
#pragma warning restore CS8618
    {
    }
}