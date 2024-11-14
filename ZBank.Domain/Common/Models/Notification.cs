using ZBank.Domain.UserAggregate.ValueObjects;

namespace ZBank.Domain.Common.Models;

public abstract class Notification : Entity<NotificationId>
{
    public string Content { get; protected set; }

    public DateTime CreatedAt { get; set; }

    public bool IsRead { get; set; }

    private NotificationSender NotificationSender { get; }
    
    protected Notification(NotificationId id, NotificationSender notificationSender, string? content = null) : base(id)
    {
        Content = content;
        IsRead = false;
        CreatedAt = DateTime.UtcNow;
        NotificationSender = notificationSender;
    }
    
    public void MarkAsRead() => IsRead = true;
}