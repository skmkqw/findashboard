using ErrorOr;
using MediatR;
using ZBank.Application.Authentication.Common;
using ZBank.Application.Common.Interfaces.Persistance;
using ZBank.Application.Common.Interfaces.Services;

namespace ZBank.Application.Authentication.Queries.Login;

public class LoginQueryHandler : IRequestHandler<LoginQuery, ErrorOr<AuthenticationResult>>
{
    private readonly IUserRepository _userRepository;

    private readonly IJwtTokenGenerator _tokenGenerator;

    public LoginQueryHandler(IUserRepository userRepository, IJwtTokenGenerator tokenGenerator)
    {
        _userRepository = userRepository;
        _tokenGenerator = tokenGenerator;
    }


    public async Task<ErrorOr<AuthenticationResult>> Handle(LoginQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.FindByEmailAsync(request.Email);
        
        if (user is null)
        {
            return Error.NotFound("User.NotFound", "User with given email not found or doesn't exist");
        }

        if (user.Password != request.Password)
        {
            return Error.Forbidden("User.InvalidCredentials", "Invalid credentials");
        }

        string token = _tokenGenerator.GenerateJwtToken(user);
        
        return new AuthenticationResult(user, token);
    }
}