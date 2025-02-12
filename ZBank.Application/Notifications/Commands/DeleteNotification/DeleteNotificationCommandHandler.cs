using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;
using ZBank.Application.Common.Interfaces.Persistance;
using ZBank.Application.Notifications.Common;
using ZBank.Domain.Common.Errors;
using ZBank.Domain.NotificationAggregate;
using ZBank.Domain.UserAggregate;

namespace ZBank.Application.Notifications.Commands.DeleteNotification;

public class DeleteNotificationCommandHandler : IRequestHandler<DeleteNotificationCommand, ErrorOr<Success>>
{
    private readonly INotificationRepository _notificationRepository;
    
    private readonly IUnitOfWork _unitOfWork;
    
    private readonly ILogger<DeleteNotificationCommandHandler> _logger;

    public DeleteNotificationCommandHandler(INotificationRepository notificationRepository,
        IUnitOfWork unitOfWork,
        ILogger<DeleteNotificationCommandHandler> logger)
    {
        _notificationRepository = notificationRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }
    
    public async Task<ErrorOr<Success>> Handle(DeleteNotificationCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Processing deleting notification for notification with id: {NotificationId}", request.NotificationId.Value);
        
        var notificationWithReceiver = await _notificationRepository.FindNotificationWithReceiverById<InformationNotification>(request.NotificationId);
        
        if (notificationWithReceiver == null)
        {
            _logger.LogInformation("Notification with ID: {Id} not found", request.NotificationId.Value);
            return Errors.Notification.InformationNotification.NotFound(request.NotificationId);
        }
        
        var (notification, receiver) = notificationWithReceiver.GetEntities();
        
        if (ValidateNotificationDelete(request, notification) is var validationResult && validationResult.IsError)
            return validationResult.Errors;
        
        await DeleteNotificationAsync(notification, receiver, cancellationToken);
        
        _logger.LogInformation("Notification with id: {NotificationId} has been deleted", notification.Id.Value);
        return Result.Success;
    }

    private Task DeleteNotificationAsync(
        InformationNotification notification,
        User receiver,
        CancellationToken cancellationToken)
    {
        _notificationRepository.DeleteNotification(notification);
        receiver.DeleteNotificationId(notification.Id);
    
        return _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    private ErrorOr<Success> ValidateNotificationDelete(
        DeleteNotificationCommand request,
        InformationNotification notification)
    {
        if (notification.CanBeModifiedBy(request.UserId))
        {
            _logger.LogInformation("User with id: {Id} can't modify this notification since he is not the receiver", request.UserId.Value);
            return Errors.Notification.InformationNotification.AccessDenied;
        }
        
        if (!notification.IsRead)
        {
            _logger.LogInformation("Notification with id: {Id} can't be deleted since it is not marked as read", notification.Id.Value);
            return Errors.Notification.InformationNotification.IsNotRead(notification.Id);
        }
        
        return Result.Success;
    }
}