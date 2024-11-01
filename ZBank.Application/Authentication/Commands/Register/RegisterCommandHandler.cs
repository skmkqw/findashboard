using ErrorOr;
using MediatR;
using ZBank.Application.Authentication.Common;
using ZBank.Application.Common.Interfaces.Persistance;
using ZBank.Application.Common.Interfaces.Services;

namespace ZBank.Application.Authentication.Commands.Register;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, ErrorOr<AuthenticationResult>>
{
    private readonly IUserRepository _userRepository;
    
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public RegisterCommandHandler(IUserRepository userRepository, IJwtTokenGenerator jwtTokenGenerator)
    {
        _userRepository = userRepository;
        _jwtTokenGenerator = jwtTokenGenerator;
    }

    public async Task<ErrorOr<AuthenticationResult>> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        if (await _userRepository.FindByEmailAsync(request.Email) != null)
        {
            return Error.Conflict("User.EmailExists", "User with given email already exists");
        }
        
        var user = new User(Guid.NewGuid(), request.FirstName, request.LastName, request.Email, request.Password);
        
        string token = _jwtTokenGenerator.GenerateJwtToken(user);
        
        _userRepository.Add(user);
        
        return new AuthenticationResult(user, token);
    }
}