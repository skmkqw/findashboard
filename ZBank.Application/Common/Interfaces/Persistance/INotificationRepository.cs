using ZBank.Domain.NotificationAggregate;
using ZBank.Domain.NotificationAggregate.ValueObjects;
using ZBank.Domain.TeamAggregate.ValueObjects;
using ZBank.Domain.UserAggregate.ValueObjects;

namespace ZBank.Application.Common.Interfaces.Persistance;

public interface INotificationRepository
{
    Task<List<Notification>> FindUserNotifications(UserId userId);
    Task<TeamInviteNotification?> FindTeamInviteNotificationById(NotificationId notificationId);
    Task<TeamInviteNotification?> FindTeamInviteNotification(UserId receiverId, TeamId teamId);

    void AddTeamInviteNotification(TeamInviteNotification teamInviteNotification);
    
    void AddInformationalNotification(InformationNotification informationNotification);
    
    void DeleteTeamInviteNotification(TeamInviteNotification teamInviteNotification);
}