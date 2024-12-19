using ZBank.Domain.NotificationAggregate.ValueObjects;
using ZBank.Domain.ProfileAggregate;
using ZBank.Domain.TeamAggregate;
using ZBank.Domain.TeamAggregate.ValueObjects;
using ZBank.Domain.UserAggregate;
using ZBank.Domain.UserAggregate.ValueObjects;
using ZBank.Domain.WalletAggregate;

namespace ZBank.Domain.NotificationAggregate.Factories;

public static class NotificationFactory
{
    public static InformationNotification CreateInformationNotification(string content, NotificationSender notificationSender, UserId receiverId)
    {
        return new InformationNotification(NotificationId.CreateUnique(), notificationSender, receiverId, content);
    }
    
    public static InformationNotification CreateSpaceCreatedNotification(User spaceOwner, PersonalSpace space)
    {
        return new InformationNotification(
            id: NotificationId.CreateUnique(),
            notificationSender: NotificationSender.Create(spaceOwner.Id, string.Join(" ", spaceOwner.FirstName, spaceOwner.LastName)),
            receiverId: spaceOwner.Id,
            content: $"Your personal space with name: {space.Name} is created successfully"
        );
    }
    
    public static InformationNotification CreateTeamCreatedNotification(User teamCreator, Team team)
    {
        return new InformationNotification(
            id: NotificationId.CreateUnique(),
            notificationSender: NotificationSender.Create(teamCreator.Id, string.Join(" ", teamCreator.FirstName, teamCreator.LastName)),
            receiverId: teamCreator.Id,
            content: $"Team '{team.Name}' is created successfully"
        );
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

    public static InformationNotification CreateProfileCreatedNotification(User profileCreator, Profile profile)
    {
        return new InformationNotification(
            id: NotificationId.CreateUnique(),
            notificationSender: NotificationSender.Create(profileCreator.Id,
                string.Join(" ", profileCreator.FirstName, profileCreator.LastName)),
            receiverId: profileCreator.Id,
            content: $"Profile '{profile.Name}' is created successfully");
    }

    public static InformationNotification CreateWalletCreatedNotification(User walletCreator, Wallet wallet)
    {
        return new InformationNotification(
            id: NotificationId.CreateUnique(),
            notificationSender: NotificationSender.Create(walletCreator.Id,
                string.Join(" ", walletCreator.FirstName, walletCreator.LastName)),
            receiverId: walletCreator.Id,
            content: $"Wallet '{wallet.Address}' is created successfully");
    }
}