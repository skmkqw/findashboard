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
    
    public async Task<List<Notification>> FindUserNotifications(UserId userId)
    {
        return await _dbContext.Notifications
            .Where(n => n.NotificationReceiverId == userId)
            .ToListAsync();
    }

    public async Task<TeamInviteNotification?> FindTeamInviteNotificationById(NotificationId notificationId)
    {
        return await _dbContext.Notifications.OfType<TeamInviteNotification>()
            .FirstOrDefaultAsync(x => x.Id == notificationId);
    }

    public async Task<TeamInviteNotification?> FindTeamInviteNotification(UserId receiverId, TeamId teamId)
    {
        return await _dbContext.Notifications.OfType<TeamInviteNotification>()
            .FirstOrDefaultAsync(n => n.NotificationReceiverId == receiverId && n.TeamId == teamId);
    }
    
    public async Task<InformationNotification?> FindInformationNotificationById(NotificationId notificationId)
    {
        return await _dbContext.Notifications.OfType<InformationNotification>()
            .FirstOrDefaultAsync(x => x.Id == notificationId);
    }
    
    public NotificationRepository(ZBankDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void AddTeamInviteNotification(TeamInviteNotification teamInviteNotification)
    {
        _dbContext.Notifications.Add(teamInviteNotification);
    }

    public void AddInformationalNotification(InformationNotification informationNotification)
    {
        _dbContext.Notifications.Add(informationNotification);
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