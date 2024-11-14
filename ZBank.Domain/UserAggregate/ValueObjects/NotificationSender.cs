using ZBank.Domain.Common.Models;

namespace ZBank.Domain.UserAggregate.ValueObjects;

public class NotificationSender : ValueObject
{
    public UserId SenderId { get; }

    public string SenderFullName { get; }
    
    private NotificationSender(UserId senderId, string senderFullName)
    {
        SenderId = senderId;
        SenderFullName = senderFullName;
    }

    public static NotificationSender Create(UserId senderId, string senderFullName)
    {
        return new NotificationSender(senderId, senderFullName); 
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return SenderId;
        yield return SenderFullName;
    }
}