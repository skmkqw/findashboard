using ZBank.Domain.UserAggregate.ValueObjects;

namespace ZBank.Domain.Common.Models;

public abstract class Notification : Entity<NotificationId>
{
    public string Content { get; protected set; }

    public DateTime CreatedAt { get; protected set; }

    public bool IsRead { get; set; }

    public UserId SenderId { get; }
    
    protected Notification(NotificationId id, string content, UserId senderId) : base(id)
    {
        Content = content;
        IsRead = false;
        CreatedAt = DateTime.UtcNow;
        SenderId = senderId;
    }
    
    public void MarkAsRead() => IsRead = true;
}