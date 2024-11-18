using ZBank.Domain.NotificationAggregate;

namespace ZBank.Application.Common.Interfaces.Persistance;

public interface INotificationRepository
{
    void AddTeamInvite(TeamInviteNotification teamInviteNotification);
}