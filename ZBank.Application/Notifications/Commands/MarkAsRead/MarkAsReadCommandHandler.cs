using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;
using ZBank.Application.Common.Interfaces.Persistance;
using ZBank.Domain.Common.Errors;
using ZBank.Domain.NotificationAggregate;
using ZBank.Domain.UserAggregate;

namespace ZBank.Application.Notifications.Commands.MarkAsRead;

public class MarkAsReadCommandHandler : IRequestHandler<MarkAsReadCommand, ErrorOr<Unit>>
{
    private readonly INotificationRepository _notificationRepository;

    
    private readonly IUnitOfWork _unitOfWork;
    
    private readonly ILogger<MarkAsReadCommandHandler> _logger;

    public MarkAsReadCommandHandler(INotificationRepository notificationRepository,
        IUnitOfWork unitOfWork,
        ILogger<MarkAsReadCommandHandler> logger)
    {
        _notificationRepository = notificationRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<ErrorOr<Unit>> Handle(MarkAsReadCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Processing mark notification as read command for user with id: {UserId} and notification with id: {NotificationId}", request.UserId.Value, request.NotificationId.Value);
        
        var notificationWithReceiver = await _notificationRepository.FindNotificationWithReceiverById<InformationNotification>(request.NotificationId);
        
        if (notificationWithReceiver == null)
        {
            _logger.LogInformation("Notification with ID: {Id} not found", request.NotificationId.Value);
            return Errors.Notification.InformationNotification.NotFound(request.NotificationId);
        }
        
        var (notification, _) = notificationWithReceiver.GetEntities();

        if (ValidateMarkAsRead(request, notification) is var validationResult && validationResult.IsError)
            return validationResult.Errors;
        
        await MarkNotificationReadAsync(notification, cancellationToken);
        
        _logger.LogInformation("Notification with id: {NotificationId} has been marked as read", notification.Id.Value);
        return Unit.Value;
    }
    
    private Task MarkNotificationReadAsync(InformationNotification notification, CancellationToken cancellationToken)
    {
        notification.MarkAsRead();

        return _unitOfWork.SaveChangesAsync(cancellationToken);
    }

    private ErrorOr<Success> ValidateMarkAsRead(
        MarkAsReadCommand request,
        InformationNotification notification)
    {
        if (notification.CanBeModifiedBy(request.UserId))
        {
            _logger.LogInformation("User with id: {Id} can't modify this notification since he is not the receiver", request.UserId.Value);
            return Errors.Notification.InformationNotification.AccessDenied;
        }
        
        if (notification.IsRead)
        {
            _logger.LogInformation("Notification with id: {Id} can't be marked as read since it is already read", notification.Id.Value);
            return Errors.Notification.InformationNotification.IsAlreadyRead(notification.Id);
        }
        
        return Result.Success;
    }
}