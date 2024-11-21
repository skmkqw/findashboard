using Mapster;
using ZBank.Application.Notifications.Commands;
using ZBank.Application.Notifications.Commands.MarkAsRead;
using ZBank.Application.Notifications.Queries.GetUserNotifications;
using ZBank.Contracts.Notifications.GetUserNotifications;
using ZBank.Domain.NotificationAggregate;
using ZBank.Domain.NotificationAggregate.ValueObjects;
using ZBank.Domain.UserAggregate.ValueObjects;

namespace ZBank.API.Mapping;

public class NotificationMappingConfigurations : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Guid?, GetUserNotificationsQuery>()
            .Map(dest => dest.UserId, src => UserId.Create(src!.Value));
        
        config.NewConfig<NotificationSender, NotificationSenderResponse>()
            .Map(dest => dest.SenderId, src => src.SenderId.Value);
        
        config.NewConfig<InformationNotification, InformationNotificationResponse>()
            .Map(dest => dest.Id, src => src.Id.Value.ToString())
            .Map(dest => dest.ReceiverId, src => src.NotificationReceiverId.Value);
        
        config.NewConfig<TeamInviteNotification, TeamInviteNotificationResponse>()
            .Map(dest => dest.Id, src => src.Id.Value.ToString())
            .Map(dest => dest.ReceiverId, src => src.NotificationReceiverId.Value)
            .Map(dest => dest.Team, src => new NotificationTeamResponse(src.TeamId.Value.ToString(), src.TeamName));

        config.NewConfig<Dictionary<string, List<Notification>>, GetUserNotificationsResponse>()
            .Map(dest => dest.InformationNotifications, src => src["InformationNotification"]
                    .Select(n => n as InformationNotification)
                    .Adapt<List<InformationNotificationResponse>>())
            .Map(dest => dest.TeamInviteNotifications, src => src["TeamInviteNotification"]
                    .Select(n => n as TeamInviteNotification)
                    .Adapt<List<TeamInviteNotificationResponse>>());
        
        config.NewConfig<(Guid userId, Guid notificationId), MarkAsReadCommand>()
            .Map(dest => dest.UserId, src => UserId.Create(src.userId))
            .Map(dest => dest.NotificationId, src => NotificationId.Create(src.notificationId));
    }
}