using ZBank.Domain.UserAggregate;

namespace ZBank.Application.Authentication.Common;

public record AuthenticationResult(
    User User,
    string Token);