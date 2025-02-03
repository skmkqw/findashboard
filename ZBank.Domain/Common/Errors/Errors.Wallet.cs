using ErrorOr;

namespace ZBank.Domain.Common.Errors;

public static partial class Errors
{
    public static class Wallet
    {
        public static Error NotFound => Error.NotFound(
            code: "Wallet.NotFound",
            description: "Wallet not found or doesn't exist");
        
        public static Error InvalidType => Error.Validation(
            code: "Wallet.InvalidType", 
            description: "Wrong wallet type.");
    }
}