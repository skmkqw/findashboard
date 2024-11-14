using ZBank.Domain.Common.Models;
using ZBank.Domain.UserAggregate.ValueObjects;

namespace ZBank.Domain.UserAggregate.Entities;

public class InformationNotification : Notification
{
    internal InformationNotification(NotificationId id, string content, UserId senderId) 
        : base(id, content, senderId)
    {
    }
}