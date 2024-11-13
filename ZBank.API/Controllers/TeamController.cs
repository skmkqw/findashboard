using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ZBank.Application.Teams.Commands.CreateTeam;
using ZBank.Contracts.Teams.CreateTeam;

namespace ZBank.API.Controllers;

[Route("api/teams")]
public class TeamController : ApiController
{
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;
    private readonly ILogger<TeamController> _logger;

    public TeamController(IMapper mapper, IMediator mediator, ILogger<TeamController> logger)
    {
        _mapper = mapper;
        _mediator = mediator;
        _logger = logger;
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateTeamRequest request)
    {
        var ownerId = GetUserId();
        if (!ownerId.HasValue)
        {
            return UnauthorizedUserIdProblem();
        }
        
        var command = _mapper.Map<CreateTeamCommand>((request, ownerId));

        var createTeamResult = await _mediator.Send(command);

        if (createTeamResult.IsError)
        {
            _logger.LogInformation("Failed to create team for emails: {Emails}.\nErrors: {Errors}", String.Join(", ", request.MemberEmails), createTeamResult.Errors);
            return Problem(createTeamResult.Errors);
        }
        
        _logger.LogInformation("Successfully created team with id: {Id}", createTeamResult.Value.Id.Value);
        return Ok(_mapper.Map<CreateTeamResponse>(createTeamResult.Value));
    }
}