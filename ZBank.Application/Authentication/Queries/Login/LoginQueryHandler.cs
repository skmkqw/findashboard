using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;
using ZBank.Application.Authentication.Common;
using ZBank.Application.Common.Interfaces.Persistance;
using ZBank.Application.Common.Interfaces.Services;
using ZBank.Application.Common.Interfaces.Services.Authentication;
using ZBank.Domain.Common.Errors;
using ZBank.Domain.UserAggregate;

namespace ZBank.Application.Authentication.Queries.Login;

public class LoginQueryHandler : IRequestHandler<LoginQuery, ErrorOr<AuthenticationResult>>
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtTokenGenerator _tokenGenerator;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ILogger<LoginQueryHandler> _logger;

    public LoginQueryHandler(
        IUserRepository userRepository, 
        IJwtTokenGenerator tokenGenerator, 
        IPasswordHasher passwordHasher, 
        ILogger<LoginQueryHandler> logger)
    {
        _userRepository = userRepository;
        _tokenGenerator = tokenGenerator;
        _passwordHasher = passwordHasher;
        _logger = logger;
    }

    public async Task<ErrorOr<AuthenticationResult>> Handle(LoginQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Login attempt started for email: {Email}", request.Email);

        if (await _userRepository.FindByEmailAsync(request.Email) is not User user)
        {
            _logger.LogInformation("Login failed: User not found for email: {Email}", request.Email);
            return Errors.Authentication.InvalidCredentials;
        }

        if (!_passwordHasher.VerifyHashedPassword(user.Password, request.Password))
        {
            _logger.LogInformation("Login failed: Invalid password for email: {Email}", request.Email);
            return Errors.Authentication.InvalidCredentials;
        }

        var token = _tokenGenerator.GenerateJwtToken(user);

        _logger.LogInformation("Login successful for email: {Email}", request.Email);
        return new AuthenticationResult(user, token);
    }
}