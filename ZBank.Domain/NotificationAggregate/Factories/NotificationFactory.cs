using ZBank.Domain.NotificationAggregate.ValueObjects;
using ZBank.Domain.TeamAggregate.ValueObjects;
using ZBank.Domain.UserAggregate.ValueObjects;

namespace ZBank.Domain.NotificationAggregate.Factories;

public static class NotificationFactory
{
    public static InformationNotification CreateInformationNotification(string content, NotificationSender notificationSender, UserId receiverId)
    {
        return new InformationNotification(NotificationId.CreateUnique(), notificationSender, receiverId, content);
    }

    public static TeamInviteNotification CreateTeamInviteNotification(NotificationSender notificationSender,
        UserId receiverId,
        TeamId teamId, 
        string teamName)
    {
        return new TeamInviteNotification(NotificationId.CreateUnique(), notificationSender, receiverId, teamId, teamName);
    }
}