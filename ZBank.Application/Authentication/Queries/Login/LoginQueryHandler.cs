using ErrorOr;
using MediatR;
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

    public LoginQueryHandler(IUserRepository userRepository, IJwtTokenGenerator tokenGenerator, IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _tokenGenerator = tokenGenerator;
        _passwordHasher = passwordHasher;
    }


    public async Task<ErrorOr<AuthenticationResult>> Handle(LoginQuery request, CancellationToken cancellationToken)
    {
        if (await _userRepository.FindByEmailAsync(request.Email) is not User user)
            return Errors.Authentication.InvalidCredentials;

        if (!_passwordHasher.VerifyHashedPassword(user.Password, request.Password))
            return Errors.Authentication.InvalidCredentials;
        
        var token = _tokenGenerator.GenerateJwtToken(user);
        
        return new AuthenticationResult(user, token);
    }
}