using ZBank.Domain.UserAggregate.Entities;
using ZBank.Domain.UserAggregate.ValueObjects;

namespace ZBank.Domain.Common.Factories;

public static class NotificationFactory
{
    public static InformationNotification CreateInformationNotification(
        string content, UserId senderId)
    {
        return new InformationNotification(NotificationId.CreateUnique(), content, senderId);
    }
}