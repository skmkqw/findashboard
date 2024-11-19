using Microsoft.EntityFrameworkCore;
using ZBank.Application.Common.Interfaces.Persistance;
using ZBank.Domain.NotificationAggregate;
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

    public void AddTeamInviteNotification(TeamInviteNotification teamInviteNotification)
    {
        _dbContext.Notifications.Add(teamInviteNotification);
    }

    public void AddInformationalNotification(InformationNotification informationNotification)
    {
        _dbContext.Notifications.Add(informationNotification);
    }

    public async Task<TeamInviteNotification?> GetTeamInviteNotification(UserId receiverId, TeamId teamId)
    {
        return await _dbContext.Notifications.OfType<TeamInviteNotification>()
            .FirstOrDefaultAsync(n => n.NotificationReceiverId == receiverId && n.TeamId == teamId);
    }
}