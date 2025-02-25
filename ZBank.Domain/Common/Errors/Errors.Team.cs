using ErrorOr;

namespace ZBank.Domain.Common.Errors;

public static partial class Errors
{
    public static class Team
    {
        public static Error NotFound => Error.NotFound(
            code: "Team.NotFound",
            description: "Team not found or doesn't exist");
        
        public static Error MemberNotExists(string email) => Error.NotFound(
            code: "Team.MemberNotExists", 
            description: $"A user with email '{email}' is not a team member");
        
        public static Error MemberAlreadyExists(string email) => Error.Conflict(
            code: "Team.MemberAlreadyExists", 
            description: $"A user with email '{email}' is already a team member");
        
        public static Error AccessDenied => Error.Unauthorized(
            code: "Team.AccessDenied", 
            description: "You are not allowed to modify this team or its resources");
    }
    
    public static class PersonalSpace
    {
        public static Error NotFound => Error.NotFound(
            code: "PersonalSpace.NotFound",
            description: "Personal space not found or doesn't exist");

        public static Error IsAlreadySet => Error.Conflict(
            code: "PersonalSpace.IsAlreadySet",
            description: "Personal space id is already set and it cannot be changed");
        
        public static Error IsNotSet => Error.NotFound(
            code: "PersonalSpace.IsNotSet",
            description: "Personal space id is not set");
        
        public static Error InvalidOperation => Error.NotFound(
            code: "PersonalSpace.InvalidOperation",
            description: "This operation is not allow on personal space");
    }
}