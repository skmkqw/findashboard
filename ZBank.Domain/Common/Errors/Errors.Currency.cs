using ErrorOr;

namespace ZBank.Domain.Common.Errors;

public static partial class Errors
{
    public static class Currency
    {
        public static Error NotFound => Error.NotFound(
            code: "Currency.NotFound",
            description: "Currency not found or doesn't exist");
    }
}