using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ZBank.API.Interfaces;
using ZBank.Application.Teams.Commands.AcceptInvite;
using ZBank.Application.Teams.Commands.CreateTeam;
using ZBank.Application.Teams.Commands.DeclineInvite;
using ZBank.Application.Teams.Commands.SendInvite;
using ZBank.Contracts.Teams.AcceptOrDeclinveInvite;
using ZBank.Contracts.Teams.CreateTeam;
using ZBank.Contracts.Teams.SendInvite;

namespace ZBank.API.Controllers;

[Route("api/teams")]
public class TeamController : ApiController
{
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;
    private readonly ILogger<TeamController> _logger;
    private readonly INotificationSender _notificationSender;

    public TeamController(
        IMapper mapper,
        IMediator mediator,
        ILogger<TeamController> logger,
        INotificationSender notificationSender)
    {
        _mapper = mapper;
        _mediator = mediator;
        _logger = logger;
        _notificationSender = notificationSender;
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
            _logger.LogInformation("Failed to create team for: {OwnerId}.\nErrors: {Errors}", ownerId, createTeamResult.Errors);
            return Problem(createTeamResult.Errors);
        }
        
        _logger.LogInformation("Successfully created team with id: {Id}", createTeamResult.Value.Result.Id.Value);

        await _notificationSender.SendInformationNotification(createTeamResult.Value.Notification);
        _logger.LogInformation("Successfully sent 'TeamCreated' notification");
        
        return Ok(_mapper.Map<CreateTeamResponse>(createTeamResult.Value.Result));
    }

    [HttpPost("invites/send")]
    public async Task<IActionResult> SendInvite(SendInviteRequest request)
    {
        var senderId = GetUserId();
        if (!senderId.HasValue)
        {
            return UnauthorizedUserIdProblem();
        }
        
        var command = _mapper.Map<SendInviteCommand>((request, senderId));
        
        var sendInviteResult = await _mediator.Send(command);

        if (sendInviteResult.IsError)
        {
            _logger.LogInformation("Failed to send team request from {SenderId} to {ReceiverEmail}. Errors: {Errors}", senderId, request.ReceiverEmail, sendInviteResult.Errors);   
            return Problem(sendInviteResult.Errors);
        }
        
        _logger.LogInformation("Successfully sent team request from {SenderId} to {ReceiverEmail}.", senderId, request.ReceiverEmail);
        
        return Ok(new SendInviteResponse($"Successfully sent team join request to {request.ReceiverEmail}."));
    }
    
    [HttpPost("invites/{inviteId}/accept")]
    public async Task<IActionResult> AcceptInvite([FromRoute] Guid inviteId)
    {
        var receiverId = GetUserId();
        if (!receiverId.HasValue)
        {
            return UnauthorizedUserIdProblem();
        }
        
        var command = _mapper.Map<AcceptInviteCommand>((receiverId, inviteId));
        
        var acceptInviteResult = await _mediator.Send(command);

        if (acceptInviteResult.IsError)
        {
            _logger.LogInformation("Failed to accept team invite {ReceiverId}. Invite id: {InviteId}. Errors: {Errors}", receiverId, inviteId, acceptInviteResult.Errors);   
            return Problem(acceptInviteResult.Errors);
        }
        
        _logger.LogInformation("Successfully accepted team invite for receiver with id: {ReceiverId}. Invite id: {InviteId}.", receiverId, inviteId);
        
        return Ok(new AcceptOrDeclineInviteResponse($"Successfully accepted team invite for receiver with id: {receiverId}."));
    }
    
    [HttpPost("invites/{inviteId}/decline")]
    public async Task<IActionResult> DeclineInvite([FromRoute] Guid inviteId)
    {
        var receiverId = GetUserId();
        if (!receiverId.HasValue)
        {
            return UnauthorizedUserIdProblem();
        }
        
        var command = _mapper.Map<DeclineInviteCommand>((receiverId, inviteId));
        
        var declineInviteResult = await _mediator.Send(command);

        if (declineInviteResult.IsError)
        {
            _logger.LogInformation("Failed to decline team invite {ReceiverId}. Invite id: {InviteId}. Errors: {Errors}", receiverId, inviteId, declineInviteResult.Errors);   
            return Problem(declineInviteResult.Errors);
        }
        
        _logger.LogInformation("Successfully declined team invite for receiver with id: {ReceiverId}. Invite id: {InviteId}.", receiverId, inviteId);
        
        return Ok(new AcceptOrDeclineInviteResponse($"Successfully declined team invite for receiver with id: {receiverId}."));
    }
}