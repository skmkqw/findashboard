using ZBank.Domain.Common.Models;
using ZBank.Domain.NotificationAggregate.ValueObjects;
using ZBank.Domain.UserAggregate.ValueObjects;

namespace ZBank.Domain.NotificationAggregate;

public abstract class Notification : Entity<NotificationId>
{
    public string Content { get; protected set; }

    public DateTime CreatedAt { get; set; }

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
        CreatedAt = DateTime.UtcNow;
        NotificationSender = notificationSender;
        NotificationReceiverId = notificationReceiverId;
    }
    
    public void MarkAsRead() => IsRead = true;
}