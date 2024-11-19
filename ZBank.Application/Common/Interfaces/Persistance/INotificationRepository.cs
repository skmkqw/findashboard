using ZBank.Domain.NotificationAggregate;
using ZBank.Domain.TeamAggregate.ValueObjects;
using ZBank.Domain.UserAggregate.ValueObjects;

namespace ZBank.Application.Common.Interfaces.Persistance;

public interface INotificationRepository
{
    void AddTeamInviteNotification(TeamInviteNotification teamInviteNotification);
    
    void AddInformationalNotification(InformationNotification informationNotification);
    
    Task<TeamInviteNotification?> GetTeamInviteNotification(UserId receiverId, TeamId teamId);
}