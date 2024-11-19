using ErrorOr;

namespace ZBank.Domain.Common.Errors;

public static partial class Errors
{
    public static class Notification
    {
        public static class TeamInvite
        {
            public static Error TeamInviteAlreadyExists(string receiverEmail, string teamName) => Error.Conflict(
                code: "Notification.TeamInvite.AlreadyExists", 
                description: $"'{receiverEmail}' is already invited to team '{teamName}'.");   
        }
    }
}