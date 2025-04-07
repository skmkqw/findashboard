using ZBank.Domain.Common.Models;
using ZBank.Domain.UserAggregate.ValueObjects;

namespace ZBank.Domain.NotificationAggregate.ValueObjects;

public class NotificationSender : ValueObject
{
    private static readonly Guid SystemSenderGuid = Guid.Parse("3a86d8eb-af4d-4537-9893-3a581cb81a8c");
    public UserId SenderId { get; }

    public string SenderFullName { get; }
    
    public bool IsSystemSender => SenderId.Value == SystemSenderGuid;

    public static readonly NotificationSender System = new(
        senderId: UserId.Create(SystemSenderGuid), 
        senderFullName: "System"
    );

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