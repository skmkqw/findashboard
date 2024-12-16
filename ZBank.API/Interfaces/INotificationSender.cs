using ZBank.Domain.NotificationAggregate;

namespace ZBank.API.Interfaces;

public interface INotificationSender
{
    Task SendInformationNotification(InformationNotification notification);
}