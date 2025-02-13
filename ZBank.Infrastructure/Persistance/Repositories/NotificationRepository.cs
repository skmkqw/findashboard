using Microsoft.EntityFrameworkCore;
using ZBank.Application.Common.Interfaces.Persistance;
using ZBank.Application.Notifications.Common;
using ZBank.Domain.Common.Models;
using ZBank.Domain.NotificationAggregate;
using ZBank.Domain.NotificationAggregate.ValueObjects;
using ZBank.Domain.TeamAggregate.ValueObjects;
using ZBank.Domain.UserAggregate.ValueObjects;

namespace ZBank.Infrastructure.Persistance.Repositories;

public class NotificationRepository : INotificationRepository
{ 
    private readonly ZBankDbContext _dbContext;
    
    public NotificationRepository(ZBankDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<T?> FindNotificationById<T>(NotificationId id) where T : Notification
    {
        return await _dbContext.Notifications.OfType<T>().FirstOrDefaultAsync(n => n.Id == id);
    }

    public async Task<NotificationValidationDetails<T>?> FindNotificationWithSenderById<T>(NotificationId notificationId) where T : Notification
    {
        var query = _dbContext.Notifications
            .OfType<T>()
            .Where(notification => notification.Id == notificationId)
            .Join(
                _dbContext.Users,
                notification => notification.NotificationSender.SenderId,
                user => user.Id,
                (notification, user) => new NotificationValidationDetails<T>(notification, user, UserRoles.Sender)
            );

        return await query.FirstOrDefaultAsync();
    }

    public async Task<NotificationValidationDetails<T>?> FindNotificationWithReceiverById<T>(NotificationId notificationId) where T : Notification
    {
        var query = _dbContext.Notifications
            .OfType<T>()
            .Where(notification => notification.Id == notificationId)
            .Join(
                _dbContext.Users,
                notification => notification.NotificationReceiverId,
                user => user.Id,
                (notification, user) => new NotificationValidationDetails<T>(notification, user, UserRoles.Receiver)
            );

        return await query.FirstOrDefaultAsync();
    }

    public async Task<List<Notification>> FindUserNotifications(UserId userId)
    {
        return await _dbContext.Notifications
            .Where(n => n.NotificationReceiverId == userId)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<TeamInviteNotification?> FindTeamInviteNotification(UserId receiverId, TeamId teamId)
    {
        return await _dbContext.Notifications.OfType<TeamInviteNotification>()
            .FirstOrDefaultAsync(n => n.NotificationReceiverId == receiverId && n.TeamId == teamId);
    }

    public void AddNotification(Notification notification)
    {
        _dbContext.Notifications.Add(notification);
    }

    public void DeleteNotification(Notification notification)
    {
        _dbContext.Notifications.Remove(notification);
    }
}