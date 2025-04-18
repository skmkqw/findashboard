using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using ZBank.Application.Authentication.Commands.Register;
using ZBank.Application.Authentication.Queries.Login;
using ZBank.Contracts.Authentication;
using LoginRequest = ZBank.Contracts.Authentication.LoginRequest;
using RegisterRequest = ZBank.Contracts.Authentication.RegisterRequest;

namespace ZBank.API.Controllers;

[AllowAnonymous]
[Route("api/auth")]
public class AuthenticationController : ApiController
{
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;
    private readonly ILogger<AuthenticationController> _logger;

    private readonly CookieOptions _cookieOptions = new()
    {
        HttpOnly = true,
        Secure = true,
        SameSite = SameSiteMode.None,
        Expires = DateTime.UtcNow.AddDays(1)
    };

    public AuthenticationController(IMapper mapper, IMediator mediator, ILogger<AuthenticationController> logger)
    {
        _mapper = mapper;
        _mediator = mediator;
        _logger = logger;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        _logger.LogInformation("Register request received for email: {Email}", request.Email);
        
        var command = _mapper.Map<RegisterCommand>(request);
        var registerUserResult = await _mediator.Send(command);

        if (registerUserResult.IsError)
        {
            _logger.LogInformation("Registration failed for email: {Email}. Errors: {Errors}", request.Email, registerUserResult.Errors);
            return Problem(registerUserResult.Errors);
        }

        _logger.LogInformation("Registration successful for email: {Email}", request.Email);
        HttpContext.Response.Cookies.Append("AuthToken", registerUserResult.Value.Token, _cookieOptions);

        return Ok(_mapper.Map<AuthenticationResponse>(registerUserResult.Value));
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        _logger.LogInformation("Login request received for email: {Email}", request.Email);
        
        var query = _mapper.Map<LoginQuery>(request);
        var loginQueryResult = await _mediator.Send(query);

        if (loginQueryResult.IsError)
        {
            _logger.LogInformation("Login failed for email: {Email}. Errors: {Errors}", request.Email, loginQueryResult.Errors);
            return Problem(loginQueryResult.Errors);
        }

        _logger.LogInformation("Login successful for email: {Email}", request.Email);
        HttpContext.Response.Cookies.Append("AuthToken", loginQueryResult.Value.Token, _cookieOptions);

        return Ok(_mapper.Map<AuthenticationResponse>(loginQueryResult.Value));
    }
    
    [HttpPost("logout")]
    public IActionResult Logout()
    {
        _logger.LogInformation("Logout request received");
    
        HttpContext.Response.Cookies.Delete("AuthToken", new CookieOptions
        {
            HttpOnly = true,
            Secure = true,
            SameSite = SameSiteMode.None,
            Path = "/",
            Domain = "localhost",
            Expires = DateTime.UtcNow.AddDays(-1)
        });

        _logger.LogInformation("Logout successful");

        return Ok(new { Message = "Logged out successfully" });
    }
}