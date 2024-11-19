using ZBank.Domain.NotificationAggregate;
using ZBank.Domain.TeamAggregate.ValueObjects;
using ZBank.Domain.UserAggregate.ValueObjects;

namespace ZBank.Application.Common.Interfaces.Persistance;

public interface INotificationRepository
{
    void AddTeamInvite(TeamInviteNotification teamInviteNotification);
    
    Task<TeamInviteNotification?> GetTeamInviteNotification(UserId receiverId, TeamId teamId);
}