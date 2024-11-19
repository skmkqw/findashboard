using ZBank.Domain.NotificationAggregate.ValueObjects;
using ZBank.Domain.TeamAggregate;
using ZBank.Domain.TeamAggregate.ValueObjects;
using ZBank.Domain.UserAggregate;
using ZBank.Domain.UserAggregate.ValueObjects;

namespace ZBank.Domain.NotificationAggregate.Factories;

public static class NotificationFactory
{
    public static InformationNotification CreateInformationNotification(string content, NotificationSender notificationSender, UserId receiverId)
    {
        return new InformationNotification(NotificationId.CreateUnique(), notificationSender, receiverId, content);
    }
    
    public static InformationNotification CreateTemInviteSentNotification(User inviteSender, User inviteReceiver, Team team)
    {
        return new InformationNotification(
            id: NotificationId.CreateUnique(),
            notificationSender: NotificationSender.Create(inviteSender.Id, string.Join(" ", inviteSender.FirstName, inviteSender.LastName)),
            receiverId: inviteSender.Id,
            content: $"{string.Join(" ", inviteReceiver.FirstName, inviteReceiver.LastName)} ({inviteReceiver.Email}) has been invited to {team.Name}"
        );
    }
    
    public static InformationNotification CreateTemInviteAcceptedNotification(User inviteSender, User inviteReceiver, Team team)
    {
        return new InformationNotification(
            id: NotificationId.CreateUnique(),
            notificationSender: NotificationSender.Create(inviteReceiver.Id, string.Join(" ", inviteReceiver.FirstName, inviteReceiver.LastName)),
            receiverId: inviteSender.Id,
            content: $"{string.Join(" ", inviteReceiver.FirstName, inviteReceiver.LastName)} ({inviteReceiver.Email}) has joined {team.Name}"
        );
    }
    
    public static InformationNotification CreateTemInviteDeclinedNotification(User inviteSender, User inviteReceiver, Team team)
    {
        return new InformationNotification(
            id: NotificationId.CreateUnique(),
            notificationSender: NotificationSender.Create(inviteReceiver.Id, string.Join(" ", inviteReceiver.FirstName, inviteReceiver.LastName)),
            receiverId: inviteSender.Id,
            content: $"{string.Join(" ", inviteReceiver.FirstName, inviteReceiver.LastName)} ({inviteReceiver.Email}) has declined join request to {team.Name}"
        );
    }

    public static TeamInviteNotification CreateTeamInviteNotification(NotificationSender notificationSender,
        UserId receiverId,
        TeamId teamId, 
        string teamName)
    {
        return new TeamInviteNotification(NotificationId.CreateUnique(), notificationSender, receiverId, teamId, teamName);
    }
}