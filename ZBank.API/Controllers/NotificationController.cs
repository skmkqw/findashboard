using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ZBank.Application.Notifications.Commands;
using ZBank.Contracts.Notifications.MarkAsRead;

namespace ZBank.API.Controllers;

[Route("api/notifications")]
public class NotificationController : ApiController
{
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;
    private readonly ILogger<NotificationController> _logger;

    public NotificationController(IMapper mapper, IMediator mediator, ILogger<NotificationController> logger)
    {
        _mapper = mapper;
        _mediator = mediator;
        _logger = logger;
    }

    [HttpPut("{notificationId}/markAsRead")]
    public async Task<IActionResult> MarkAsRead([FromRoute] Guid notificationId)
    {
        var userId = GetUserId();
        if (!userId.HasValue)
        {
            return UnauthorizedUserIdProblem();
        }
        
        var command = _mapper.Map<MarkAsReadCommand>((userId.Value, notificationId));
        
        var markAsReadResult = await _mediator.Send(command);
        
        if (markAsReadResult.IsError)
        {
            _logger.LogInformation("Failed to mark notification as read for notification with id: {NotificationId}", notificationId);   
            return Problem(markAsReadResult.Errors);
        }
        
        _logger.LogInformation("Successfully marked notification as read for notification with id: {NotificationId}", notificationId);
        
        return Ok(new MarkAsReadResponse($"Successfully marked notification with id '{notificationId}' as read"));
    }
}