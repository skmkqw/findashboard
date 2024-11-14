using ZBank.Domain.TeamAggregate.ValueObjects;
using ZBank.Domain.UserAggregate.Entities;
using ZBank.Domain.UserAggregate.ValueObjects;

namespace ZBank.Domain.Common.Factories;

public static class NotificationFactory
{
    public static InformationNotification CreateInformationNotification(string content, NotificationSender notificationSender)
    {
        return new InformationNotification(NotificationId.CreateUnique(), content, notificationSender);
    }

    public static TeamInviteNotification CreateTeamInviteNotification(NotificationSender notificationSender,
        TeamId teamId, string teamName)
    {
        return new TeamInviteNotification(NotificationId.CreateUnique(), notificationSender, teamId, teamName);
    }
}