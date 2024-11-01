using ErrorOr;
using MediatR;
using ZBank.Application.Authentication.Common;
using ZBank.Application.Common.Interfaces.Persistance;
using ZBank.Application.Common.Interfaces.Services;
using ZBank.Application.Common.Interfaces.Services.Authentication;
using ZBank.Domain.Common.Errors;
using ZBank.Domain.UserAggregate;

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
        if (await _userRepository.FindByEmailAsync(request.Email) is not null)
            return Errors.User.DuplicateEmail;
        
        string hashedPassword = _passwordHasher.HashPassword(request.Password);    
    
        var user = User.Create(
            request.FirstName,
            request.LastName,
            request.Email,
            hashedPassword);
        
        string token = _jwtTokenGenerator.GenerateJwtToken(user);
        
        _userRepository.Add(user);
        
        return new AuthenticationResult(user, token);
    }
}