using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ZBank.Application.Notifications.Commands;
using ZBank.Application.Notifications.Commands.DeleteNotification;
using ZBank.Application.Notifications.Commands.MarkAsRead;
using ZBank.Contracts.Notifications.DeleteNotification;
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
    
    [HttpDelete("{notificationId}")]
    public async Task<IActionResult> Delete([FromRoute] Guid notificationId)
    {
        var userId = GetUserId();
        if (!userId.HasValue)
        {
            return UnauthorizedUserIdProblem();
        }
        
        var command = _mapper.Map<DeleteNotificationCommand>((userId.Value, notificationId));
        
        var deleteNotificationResult = await _mediator.Send(command);
        
        if (deleteNotificationResult.IsError)
        {
            _logger.LogInformation("Failed to delete notification with id: {NotificationId}", notificationId);   
            return Problem(deleteNotificationResult.Errors);
        }
        
        _logger.LogInformation("Successfully deleted notification with id: {NotificationId}", notificationId);
        
        return Ok(new DeleteNotificationResponse($"Successfully deleted notification with id '{notificationId}'"));
    }
}