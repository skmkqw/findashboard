using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;
using ZBank.Application.Authentication.Common;
using ZBank.Application.Common.Interfaces.Persistance;
using ZBank.Application.Common.Interfaces.Services.Authentication;
using ZBank.Domain.Common.Errors;
using ZBank.Domain.UserAggregate;

namespace ZBank.Application.Authentication.Commands.Register;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, ErrorOr<AuthenticationResult>>
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger<RegisterCommandHandler> _logger;

    public RegisterCommandHandler(
        IUserRepository userRepository,
        IJwtTokenGenerator jwtTokenGenerator,
        IPasswordHasher passwordHasher,
        IUnitOfWork unitOfWork,
        ILogger<RegisterCommandHandler> logger)
    {
        _userRepository = userRepository;
        _jwtTokenGenerator = jwtTokenGenerator;
        _passwordHasher = passwordHasher;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<ErrorOr<AuthenticationResult>> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling registration for email: {Email}", request.Email);

        if (await _userRepository.FindByEmailAsync(request.Email) is not null)
        {
            _logger.LogWarning("Duplicate email found: {Email}", request.Email);
            return Errors.User.DuplicateEmail;
        }

        string hashedPassword = _passwordHasher.HashPassword(request.Password);
        var user = User.Create(request.FirstName, request.LastName, request.Email, hashedPassword);

        string token = _jwtTokenGenerator.GenerateJwtToken(user);
        _userRepository.Add(user);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("User registered successfully: {Email}", request.Email);
        return new AuthenticationResult(user, token);
    }
}