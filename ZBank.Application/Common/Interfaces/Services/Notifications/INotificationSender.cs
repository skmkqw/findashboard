using ZBank.Domain.NotificationAggregate;

namespace ZBank.Application.Common.Interfaces.Services.Notifications;

public interface INotificationSender
{
    Task SendInformationNotification(InformationNotification notification);
    
    Task SendTeamInviteNotification(TeamInviteNotification notification);
}