using ZBank.Domain.NotificationAggregate;
using ZBank.Domain.NotificationAggregate.ValueObjects;
using ZBank.Domain.TeamAggregate.ValueObjects;
using ZBank.Domain.UserAggregate.ValueObjects;

namespace ZBank.Application.Common.Interfaces.Persistance;

public interface INotificationRepository
{
    Task<T?> FindNotificationById<T>(NotificationId id) where T : Notification;
    
    Task<List<Notification>> FindUserNotifications(UserId userId);
    
    Task<TeamInviteNotification?> FindTeamInviteNotification(UserId receiverId, TeamId teamId);
    
    void AddNotification(Notification notification);
    
    void DeleteTeamInviteNotification(TeamInviteNotification teamInviteNotification);
    
    void DeleteInformationNotification(InformationNotification informationNotification);
}