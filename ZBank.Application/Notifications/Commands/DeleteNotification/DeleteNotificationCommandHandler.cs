using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;
using ZBank.Application.Common.Interfaces.Persistance;
using ZBank.Domain.Common.Errors;

namespace ZBank.Application.Notifications.Commands.DeleteNotification;

public class DeleteNotificationCommandHandler : IRequestHandler<DeleteNotificationCommand, ErrorOr<Unit>>
{
    private readonly INotificationRepository _notificationRepository;

    private readonly IUserRepository _userRepository;
    
    private readonly IUnitOfWork _unitOfWork;
    
    private readonly ILogger<DeleteNotificationCommandHandler> _logger;

    public DeleteNotificationCommandHandler(INotificationRepository notificationRepository,
        IUserRepository userRepository,
        IUnitOfWork unitOfWork,
        ILogger<DeleteNotificationCommandHandler> logger)
    {
        _notificationRepository = notificationRepository;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }
    
    public async Task<ErrorOr<Unit>> Handle(DeleteNotificationCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Processing deleting notification for notification with id: {NotificationId}", request.NotificationId.Value);
        
        var user = await _userRepository.FindByIdAsync(request.UserId);

        if (user is null)
        {
            _logger.LogInformation("User with ID: {Id} not found", request.UserId.Value);
            return Errors.User.IdNotFound(request.UserId);
        }
        
        var notification = await _notificationRepository.FindInformationNotificationById(request.NotificationId);

        if (notification is null)
        {
            _logger.LogInformation("Notification with ID: {Id} not found", request.UserId.Value);
            return Errors.Notification.InformationNotification.NotFound(request.NotificationId);
        }

        if (notification.NotificationReceiverId != user.Id)
        {
            _logger.LogInformation("User with id: {Id} can't modify this notification since he is not the receiver", user.Id.Value);
            return Errors.Notification.InformationNotification.AccessDenied;
        }
        
        if (!notification.IsRead)
        {
            _logger.LogInformation("Notification with id: {Id} can't be deleted since it is not marked as read", notification.Id.Value);
            return Errors.Notification.InformationNotification.IsNotRead(notification.Id);
        }
        
        _notificationRepository.DeleteInformationNotification(notification);

        user.DeleteNotificationId(notification.Id);
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        _logger.LogInformation("Notification with id: {NotificationId} has been deleted", notification.Id.Value);
        return Unit.Value;
    }
}