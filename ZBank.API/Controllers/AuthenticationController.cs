using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ZBank.Application.Authentication.Commands.Register;
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
    
}