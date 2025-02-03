using ErrorOr;

namespace ZBank.Domain.Common.Errors;

public static partial class Errors
{
    public static class Profile
    {
        public static Error NotFound => Error.NotFound(
            code: "Profile.NotFound",
            description: "Profile not found or doesn't exist");
        
        public static Error AccessDenied => Error.Unauthorized(
            code: "Profile.AccessDenied", 
            description: "You are not allowed to modify this profile or its resources");
    }
}