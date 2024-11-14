using ZBank.Domain.Common.Models;
using ZBank.Domain.TeamAggregate.ValueObjects;
using ZBank.Domain.UserAggregate.ValueObjects;

namespace ZBank.Domain.UserAggregate.Entities;

public class TeamInviteNotification : Notification
{
    public TeamId TeamId { get; }

    public string TeamName { get; }
    
    internal TeamInviteNotification(NotificationId id,
        NotificationSender notificationSender,
        TeamId teamId, string teamName) : base(id, notificationSender)
    {
        TeamId = teamId;
        TeamName = teamName;
        Content = $"{notificationSender.SenderFullName} has invited you to team: {teamName}";
    }
}