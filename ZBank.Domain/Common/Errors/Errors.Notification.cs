using ErrorOr;
using ZBank.Domain.NotificationAggregate.ValueObjects;

namespace ZBank.Domain.Common.Errors;

public static partial class Errors
{
    public static class Notification
    {
        public static class InformationNotification
        {
            public static Error AccessDenied => Error.Unauthorized(
                code: "Notification.InformationNotification.AccessDenied",
                description: "You are not allowed to modify this notification.");
            public static Error NotFound(NotificationId notificationId) => Error.NotFound(
                code: "Notification.InformationNotification.NotFound",
                description: $"Informational notification with id '{notificationId.Value.ToString()}' not found or doesn't exist.");
            
            public static Error IsAlreadyRead(NotificationId notificationId) => Error.Conflict(
                code: "Notification.InformationNotification.IsAlreadyRead",
                description: $"Informational notification with id '{notificationId.Value.ToString()}' is already marked as read.");
            
            public static Error IsNotRead(NotificationId notificationId) => Error.Conflict(
                code: "Notification.InformationNotification.IsNotRead",
                description: $"Informational notification with id '{notificationId.Value.ToString()}' is not marked as read.");
        }
        
        public static class TeamInvite
        {
            public static Error AccessDenied => Error.Unauthorized(
                code: "Notification.TeamInvite.AccessDenied",
                description: "You are not allowed to accept/decline this request");
            public static Error NotFound(NotificationId notificationId) => Error.NotFound(
                code: "Notification.TeamInvite.NotFound",
                description: $"Join team request with id '{notificationId.Value.ToString()}' not found or doesn't exist.");
            
            public static Error TeamInviteAlreadyExists(string receiverEmail, string teamName) => Error.Conflict(
                code: "Notification.TeamInvite.AlreadyExists", 
                description: $"'{receiverEmail}' is already invited to team '{teamName}'.");   
        }
    }
}