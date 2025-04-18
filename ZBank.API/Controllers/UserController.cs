using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ZBank.Application.Notifications.Queries.GetUserNotifications;
using ZBank.Application.Users.Queries.GetUserTeams;
using ZBank.Contracts.Notifications.GetUserNotifications;
using ZBank.Contracts.Teams.GetTeams;

namespace ZBank.API.Controllers;

[Route("api/users")]
public class UserController : ApiController
{
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;
    private readonly ILogger<UserController> _logger;

    public UserController(IMapper mapper, IMediator mediator, ILogger<UserController> logger)
    {
        _mapper = mapper;
        _mediator = mediator;
        _logger = logger;
    }

    [HttpGet("notifications")]
    public async Task<IActionResult> GetUserNotifications()
    {
        var userId = GetUserId();
        if (!userId.HasValue)
        {
            return UnauthorizedUserIdProblem();
        }
        
        var query = _mapper.Map<GetUserNotificationsQuery>(userId.Value);
        
        var getUserNotificationsResult = await _mediator.Send(query);
        
        if (getUserNotificationsResult.IsError)
        {
            _logger.LogInformation("Failed to fetch notifications for user with id: {UserId}", userId);   
            return Problem(getUserNotificationsResult.Errors);
        }
        
        _logger.LogInformation("Successfully fetched notifications for user with id: {UserId}", userId);
        
        return Ok(_mapper.Map<GetUserNotificationsResponse>(getUserNotificationsResult.Value));
    }

    [HttpGet("teams")]
    public async Task<IActionResult> GetUserTeams()
    {
        var userId = GetUserId();
        if (!userId.HasValue)
        {
            return UnauthorizedUserIdProblem();
        }
        
        var query = _mapper.Map<GetUserTeamsQuery>(userId.Value);
        
        var getUserTeamsResult = await _mediator.Send(query);
        
        if (getUserTeamsResult.IsError)
        {
            _logger.LogInformation("Failed to fetch teams for user with id: {UserId}", userId);   
            return Problem(getUserTeamsResult.Errors);
        }
        
        _logger.LogInformation("Successfully fetched teams for user with id: {UserId}", userId);
        
        return Ok(_mapper.Map<GetUserTeamsResponse>(getUserTeamsResult.Value));
    }
}