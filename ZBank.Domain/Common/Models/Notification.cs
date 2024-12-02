using ZBank.Domain.NotificationAggregate.ValueObjects;
using ZBank.Domain.UserAggregate.ValueObjects;

namespace ZBank.Domain.Common.Models;

public abstract class Notification : AggregateRoot<NotificationId>
{
    public string Content { get; protected set; }
    
    public bool IsRead { get; set; }

    public NotificationSender NotificationSender { get; }
    
    public UserId NotificationReceiverId { get; }
    
    protected Notification(NotificationId id,
        NotificationSender notificationSender,
        UserId notificationReceiverId,
        string? content = null) : base(id)
    {
        Content = content ?? string.Empty;
        IsRead = false;
        NotificationSender = notificationSender;
        NotificationReceiverId = notificationReceiverId;
    }
    
    public void MarkAsRead() => IsRead = true;
    
#pragma warning disable CS8618
    protected Notification()
#pragma warning restore CS8618
    {
    }
}