using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZBank.Application.Authentication.Commands.Register;
using ZBank.Application.Authentication.Queries.Login;
using ZBank.Contracts.Authentication;

namespace ZBank.API.Controllers;

[AllowAnonymous]
[Route("api/auth")]
public class AuthenticationController : ApiController
{
    private readonly IMapper _mapper;
    
    private readonly IMediator _mediator;

    public AuthenticationController(IMapper mapper, IMediator mediator)
    {
        _mapper = mapper;
        _mediator = mediator;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        var command = _mapper.Map<RegisterCommand>(request);
        
        var registerUserResult = await _mediator.Send(command);
        
        return registerUserResult.Match(
            onValue: value => Ok(_mapper.Map<AuthenticationResponse>(value)),
            onError: errors => Problem(errors)
        );
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var query = _mapper.Map<LoginQuery>(request);
        
        var loginQueryResult = await _mediator.Send(query);

        return loginQueryResult.Match(
            onValue: value => Ok(_mapper.Map<AuthenticationResponse>(value)),
            onError: errors => Problem(errors)
        );
    }
    
}