using ZBank.Domain.NotificationAggregate;
using ZBank.Domain.NotificationAggregate.ValueObjects;
using ZBank.Domain.TeamAggregate.ValueObjects;
using ZBank.Domain.UserAggregate.ValueObjects;

namespace ZBank.Application.Common.Interfaces.Persistance;

public interface INotificationRepository
{
    Task<List<Notification>> FindUserNotifications(UserId userId);
    
    Task<T?> FindNotificationById<T>(NotificationId id) where T : Notification;
    
    Task<TeamInviteNotification?> FindTeamInviteNotification(UserId receiverId, TeamId teamId);
    
    void AddTeamInviteNotification(TeamInviteNotification teamInviteNotification);
    
    void AddInformationalNotification(InformationNotification informationNotification);
    
    void DeleteTeamInviteNotification(TeamInviteNotification teamInviteNotification);
    
    void DeleteInformationNotification(InformationNotification informationNotification);
}