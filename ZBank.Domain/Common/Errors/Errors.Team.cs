using ErrorOr;

namespace ZBank.Domain.Common.Errors;

public static partial class Errors
{
    public static class Team
    {
        public static Error NotFound => Error.NotFound(
            code: "Team.NotFound",
            description: "Team not found or doesn't exist");
        
        public static Error MemberExists(string email) => Error.Conflict(
            code: "Team.MemberExists", 
            description: $"A user with email '{email}' is already a team member");
    }
}