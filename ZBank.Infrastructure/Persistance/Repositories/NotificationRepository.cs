using Microsoft.EntityFrameworkCore;
using ZBank.Application.Common.Interfaces.Persistance;
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
    
    public async Task<List<Notification>> FindUserNotifications(UserId userId)
    {
        return await _dbContext.Notifications
            .Where(n => n.NotificationReceiverId == userId)
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

    public void DeleteTeamInviteNotification(TeamInviteNotification teamInviteNotification)
    {
        _dbContext.Notifications.Remove(teamInviteNotification);
    }

    public void DeleteInformationNotification(InformationNotification informationNotification)
    {
        _dbContext.Notifications.Remove(informationNotification);
    }
}