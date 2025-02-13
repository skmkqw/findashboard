using ZBank.Application.Notifications.Common;
using ZBank.Domain.Common.Models;
using ZBank.Domain.NotificationAggregate;
using ZBank.Domain.NotificationAggregate.ValueObjects;
using ZBank.Domain.TeamAggregate.ValueObjects;
using ZBank.Domain.UserAggregate.ValueObjects;

namespace ZBank.Application.Common.Interfaces.Persistance;

public interface INotificationRepository
{
    Task<T?> FindNotificationById<T>(NotificationId id) where T : Notification;
    
    Task<NotificationValidationDetails<T>?> FindNotificationWithSenderById<T>(NotificationId notificationId) where T : Notification;
    
    Task<NotificationValidationDetails<T>?> FindNotificationWithReceiverById<T>(NotificationId notificationId) where T : Notification;
    
    Task<List<Notification>> FindUserNotifications(UserId userId);
    
    Task<TeamInviteNotification?> FindTeamInviteNotification(UserId receiverId, TeamId teamId);
    
    void AddNotification(Notification notification);
    
    void DeleteNotification(Notification notification);
}