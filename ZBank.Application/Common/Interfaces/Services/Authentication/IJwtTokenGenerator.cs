using ZBank.Application.Authentication.Common;

namespace ZBank.Application.Common.Interfaces.Services.Authentication;

public interface IJwtTokenGenerator
{
    string GenerateJwtToken(User user);
}