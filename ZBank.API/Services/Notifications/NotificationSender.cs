using MapsterMapper;
using Microsoft.AspNetCore.SignalR;
using ZBank.API.Hubs;
using ZBank.API.Interfaces;
using ZBank.Application.Common.Interfaces.Services;
using ZBank.Contracts.Notifications.GetUserNotifications;
using ZBank.Domain.NotificationAggregate;

namespace ZBank.API.Services.Notifications;

public class NotificationSender<T> : INotificationSender where T : Hub<INotificationClient>
{
    private readonly IMapper _mapper;
    
    private readonly IUserConnectionManager _connectionManager;

    private readonly IHubContext<T, INotificationClient> _notificationHubContext;

    public NotificationSender(IMapper mapper,
        IUserConnectionManager connectionManager,
        IHubContext<T, INotificationClient> notificationHubContext)
    {
        _mapper = mapper;
        _connectionManager = connectionManager;
        _notificationHubContext = notificationHubContext;
    }
    
    public async Task SendInformationNotification(InformationNotification notification)
    {
        var receiverId = notification.NotificationReceiverId;
        var connections = _connectionManager.GetConnections(receiverId);

        if (connections != null)
        {
            foreach (var connectionId in connections)
            {
                await _notificationHubContext.Clients.Client(connectionId)
                    .ReceiveInformationNotification(_mapper.Map<InformationNotificationResponse>(notification));
            }
        }
    }
}