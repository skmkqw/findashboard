using ZBank.Domain.UserAggregate;

namespace ZBank.Application.Common.Interfaces.Services.Authentication;

public interface IJwtTokenGenerator
{
    string GenerateJwtToken(User user);
}