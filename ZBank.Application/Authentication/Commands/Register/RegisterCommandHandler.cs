using ErrorOr;
using MediatR;
using ZBank.Application.Authentication.Common;
using ZBank.Application.Common.Interfaces.Persistance;
using ZBank.Application.Common.Interfaces.Services;
using ZBank.Application.Common.Interfaces.Services.Authentication;

namespace ZBank.Application.Authentication.Commands.Register;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, ErrorOr<AuthenticationResult>>
{
    private readonly IUserRepository _userRepository;
    
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    
    private readonly IPasswordHasher _passwordHasher;

    public RegisterCommandHandler(IUserRepository userRepository, IJwtTokenGenerator jwtTokenGenerator, IPasswordHasher passwordHasher)
    {
        _userRepository = userRepository;
        _jwtTokenGenerator = jwtTokenGenerator;
        _passwordHasher = passwordHasher;
    }

    public async Task<ErrorOr<AuthenticationResult>> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        if (await _userRepository.FindByEmailAsync(request.Email) != null)
        {
            return Error.Conflict("User.EmailExists", "User with given email already exists");
        }
        
        var user = new User(Guid.NewGuid(), request.FirstName, request.LastName, request.Email, _passwordHasher.HashPassword(request.Password));
        
        string token = _jwtTokenGenerator.GenerateJwtToken(user);
        
        _userRepository.Add(user);
        
        return new AuthenticationResult(user, token);
    }
}