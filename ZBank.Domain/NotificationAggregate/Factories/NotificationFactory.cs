using ZBank.Domain.NotificationAggregate.ValueObjects;
using ZBank.Domain.ProfileAggregate;
using ZBank.Domain.TeamAggregate;
using ZBank.Domain.TeamAggregate.ValueObjects;
using ZBank.Domain.UserAggregate;
using ZBank.Domain.UserAggregate.ValueObjects;
using ZBank.Domain.WalletAggregate;
using ZBank.Domain.WalletAggregate.Entities;

namespace ZBank.Domain.NotificationAggregate.Factories;

public static class NotificationFactory
{
    public static InformationNotification CreateSpaceCreatedNotification(UserId receiverId, PersonalSpace space)
    {
        return new InformationNotification(
            id: NotificationId.CreateUnique(),
            notificationSender: NotificationSender.System, 
            receiverId: receiverId,
            content: $"Your personal space with name: {space.Name} is created successfully"
        );
    }
    
    public static InformationNotification CreateTeamCreatedNotification(UserId teamCreatorId, Team team)
    {
        return new InformationNotification(
            id: NotificationId.CreateUnique(),
            notificationSender: NotificationSender.System,
            receiverId: teamCreatorId,
            content: $"Team '{team.Name}' is created successfully"
        );
    }
    
    public static InformationNotification CreateTemInviteSentNotification(UserId inviteSenderId, User inviteReceiver, Team team)
    {
        return new InformationNotification(
            id: NotificationId.CreateUnique(),
            notificationSender: NotificationSender.System,
            receiverId: inviteSenderId,
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
    
    public static InformationNotification CreateBalanceAddedNotification(User balanceCreator, Balance balance, string walletAddress)
    {
        return new InformationNotification(
            id: NotificationId.CreateUnique(),
            notificationSender: NotificationSender.Create(balanceCreator.Id,
                string.Join(" ", balanceCreator.FirstName, balanceCreator.LastName)),
            receiverId: balanceCreator.Id,
            content: $"Balance for wallet '{walletAddress}' is created successfully");
    }
}