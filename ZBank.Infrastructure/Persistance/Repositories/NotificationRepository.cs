using ZBank.Application.Common.Interfaces.Persistance;
using ZBank.Domain.NotificationAggregate;

namespace ZBank.Infrastructure.Persistance.Repositories;

public class NotificationRepository : INotificationRepository
{ 
    private readonly ZBankDbContext _dbContext;

    public NotificationRepository(ZBankDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public void AddTeamInvite(TeamInviteNotification teamInviteNotification)
    {
        _dbContext.Notifications.Add(teamInviteNotification);
    }
}