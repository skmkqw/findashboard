using ErrorOr;
using ZBank.Domain.NotificationAggregate.ValueObjects;

namespace ZBank.Domain.Common.Errors;

public static partial class Errors
{
    public static class Notification
    {
        public static class TeamInvite
        {
            public static Error AccessDenied => Error.Unauthorized(
                code: "Notification.TeamInvite.AccessDenied",
                description: "You are not allowed to accept/decline this request");
            public static Error TeamInviteNotFound(NotificationId notificationId) => Error.NotFound(
                code: "Notification.TeamInvite.NotFound",
                description: $"Join team request with id '{notificationId.Value.ToString()}' not found or doesn't exist.");
            
            public static Error TeamInviteAlreadyExists(string receiverEmail, string teamName) => Error.Conflict(
                code: "Notification.TeamInvite.AlreadyExists", 
                description: $"'{receiverEmail}' is already invited to team '{teamName}'.");   
        }
    }
}