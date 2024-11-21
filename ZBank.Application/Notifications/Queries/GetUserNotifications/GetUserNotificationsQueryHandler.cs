using System.Reflection;
using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;
using ZBank.Application.Common.Interfaces.Persistance;
using ZBank.Domain.Common.Errors;
using ZBank.Domain.NotificationAggregate;

namespace ZBank.Application.Notifications.Queries.GetUserNotifications;

public class GetUserNotificationsQueryHandler : IRequestHandler<GetUserNotificationsQuery, ErrorOr<Dictionary<string, List<Notification>>>>
{
    private readonly INotificationRepository _notificationRepository;
    
    private readonly IUserRepository _userRepository;
    
    private readonly ILogger<GetUserNotificationsQueryHandler> _logger;


    public GetUserNotificationsQueryHandler(INotificationRepository notificationRepository, IUserRepository userRepository, ILogger<GetUserNotificationsQueryHandler> logger)
    {
        _notificationRepository = notificationRepository;
        _logger = logger;
        _userRepository = userRepository;
    }

    public async Task<ErrorOr<Dictionary<string, List<Notification>>>> Handle(GetUserNotificationsQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Processing get notifications query for user with id: {UserId}", request.UserId.Value);
        
        var user = await _userRepository.FindByIdAsync(request.UserId);

        if (user is null)
        {
            _logger.LogInformation("User with ID: {Id} not found", request.UserId.Value);
            return Errors.User.IdNotFound(request.UserId);
        }
        
        var assembly = Assembly.GetAssembly(typeof(Notification));

        if (assembly is null)
        {
            _logger.LogWarning("Assembly not found");
            return Error.Unexpected("Unexpected.AssemblyNotFound", "Assembly not found");
        }
            
        var notificationTypes = assembly.GetTypes().Where(t => t.IsClass && !t.IsAbstract && t.IsSubclassOf(typeof(Notification)));
        
        var result = new Dictionary<string, List<Notification>>();
        
        var notifications = await _notificationRepository.FindUserNotifications(user.Id);

        foreach (var notificationType in notificationTypes)
        {
            result[notificationType.Name] = notifications.Where(n => n.GetType() == notificationType).ToList();
        }
        
        _logger.LogInformation("Successfully found {Count} notifications", notifications.Count);
        return result;
    }
}