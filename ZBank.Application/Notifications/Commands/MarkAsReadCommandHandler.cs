using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;
using ZBank.Application.Common.Interfaces.Persistance;
using ZBank.Domain.Common.Errors;
using ZBank.Domain.UserAggregate;

namespace ZBank.Application.Notifications.Commands;

public class MarkAsReadCommandHandler : IRequestHandler<MarkAsReadCommand, ErrorOr<Unit>>
{
    private readonly INotificationRepository _notificationRepository;

    private readonly IUserRepository _userRepository;
    
    private readonly IUnitOfWork _unitOfWork;
    
    private readonly ILogger<MarkAsReadCommandHandler> _logger;

    public MarkAsReadCommandHandler(INotificationRepository notificationRepository,
        IUserRepository userRepository,
        IUnitOfWork unitOfWork,
        ILogger<MarkAsReadCommandHandler> logger)
    {
        _notificationRepository = notificationRepository;
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<ErrorOr<Unit>> Handle(MarkAsReadCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Processing mark notification as read command for user with id: {UserId} and notification with id: {NotificationId}", request.UserId.Value, request.NotificationId.Value);
        
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
        
        if (notification.IsRead)
        {
            _logger.LogInformation("Notification with id: {Id} can't be marked as read since it is already read", notification.Id.Value);
            return Errors.Notification.InformationNotification.IsAlreadyRead(notification.Id);
        }
        
        notification.MarkAsRead();

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        _logger.LogInformation("Notification with id: {NotificationId} has been marked as read", notification.Id.Value);
        return Unit.Value;
    }
}