using ZBank.Domain.Common.Models;
using ZBank.Domain.NotificationAggregate.ValueObjects;
using ZBank.Domain.UserAggregate;
using ZBank.Domain.UserAggregate.ValueObjects;

namespace ZBank.Application.Common.Models.Validation;

public enum UserRoles
{
    Sender,
    Receiver
} 

public class NotificationValidationDetails<T> where T : Notification
{
    private readonly T _notification;
    
    private readonly User _user;
    
    public NotificationValidationDetails(T notification, User user, UserRoles role)
    {
        _notification = notification;
        _user = user;
        Role = role;
    }
    
    public UserRoles Role { get; init; }
    
    public bool IsRead => _notification.IsRead;

    public NotificationId NotificationId => _notification.Id;
    
    public UserId ReceiverId => _notification.NotificationReceiverId;
    
    public UserId SenderId => _notification.NotificationSender.SenderId;
    
    public (T Notification, User User) GetEntities() => (_notification, _user);
}