using ZBank.Domain.NotificationAggregate.ValueObjects;
using ZBank.Domain.UserAggregate.ValueObjects;

namespace ZBank.Domain.NotificationAggregate;

public class InformationNotification : Notification
{
    internal InformationNotification(NotificationId id, NotificationSender notificationSender, UserId receiverId, string content) 
        : base(id, notificationSender, receiverId, content)
    {
    }
    
#pragma warning disable CS8618
    private InformationNotification()
#pragma warning restore CS8618
    {
    }
}