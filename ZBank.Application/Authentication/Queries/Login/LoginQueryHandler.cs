using ErrorOr;
using MediatR;
using ZBank.Application.Authentication.Common;
using ZBank.Application.Common.Interfaces.Persistance;
using ZBank.Application.Common.Interfaces.Services;
using ZBank.Application.Common.Interfaces.Services.Authentication;

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
        var user = await _userRepository.FindByEmailAsync(request.Email);
        
        if (user is null)
        {
            return Error.NotFound("User.NotFound", "User with given email not found or doesn't exist");
        }

        if (!_passwordHasher.VerifyHashedPassword(user.Password, request.Password))
        {
            return Error.Forbidden("User.InvalidCredentials", "Invalid credentials");
        }

        string token = _tokenGenerator.GenerateJwtToken(user);
        
        return new AuthenticationResult(user, token);
    }
}