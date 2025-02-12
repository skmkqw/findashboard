using System.Reflection;
using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;
using ZBank.Application.Common.Interfaces.Persistance;
using ZBank.Domain.Common.Errors;
using ZBank.Domain.Common.Models;
using ZBank.Domain.NotificationAggregate;
using ZBank.Domain.UserAggregate.ValueObjects;

namespace ZBank.Application.Notifications.Queries.GetUserNotifications;

public class GetUserNotificationsQueryHandler : IRequestHandler<GetUserNotificationsQuery, ErrorOr<Dictionary<string, List<Notification>>>>
{
    private readonly INotificationRepository _notificationRepository;
    
    private readonly ILogger<GetUserNotificationsQueryHandler> _logger;
    
    public GetUserNotificationsQueryHandler(INotificationRepository notificationRepository, ILogger<GetUserNotificationsQueryHandler> logger)
    {
        _notificationRepository = notificationRepository;
        _logger = logger;
    }

    public async Task<ErrorOr<Dictionary<string, List<Notification>>>> Handle(GetUserNotificationsQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Processing get notifications query for user with id: {UserId}", request.UserId.Value);
        
        var getNotificationTypesResult = GetNotificationTypesFromAssembly();
        
        if (getNotificationTypesResult.IsError)
            return getNotificationTypesResult.Errors;

        var notifications = await GetUserNotificationsAsync(request.UserId, getNotificationTypesResult.Value);
        
        _logger.LogInformation("Successfully found {Count} notifications", notifications.Count);
        return notifications;
    }

    private async Task<Dictionary<string, List<Notification>>> GetUserNotificationsAsync(UserId userId, IEnumerable<Type> notificationTypes)
    {
        var result = new Dictionary<string, List<Notification>>();
        
        var notifications = await _notificationRepository.FindUserNotifications(userId);

        foreach (var notificationType in notificationTypes)
        {
            result[notificationType.Name] = notifications.Where(n => n.GetType() == notificationType).ToList();
        }
        
        return result;
    }

    private ErrorOr<IEnumerable<Type>> GetNotificationTypesFromAssembly()
    {
        var assembly = Assembly.GetAssembly(typeof(Notification));

        if (assembly is null)
        {
            _logger.LogError("Assembly not found");
            return Error.Unexpected("Unexpected.AssemblyNotFound", "Assembly not found");
        }
            
        return assembly.GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract && t.IsSubclassOf(typeof(Notification)))
            .ToList();
    }
}