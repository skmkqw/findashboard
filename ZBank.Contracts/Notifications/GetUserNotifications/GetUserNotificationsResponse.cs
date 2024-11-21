namespace ZBank.Contracts.Notifications.GetUserNotifications;

public record NotificationSenderResponse(string SenderId, string SenderFullName);

public record NotificationTeamResponse(string TeamId, string TeamName);

public record InformationNotificationResponse(string Content, bool IsRead, string ReceiverId, DateTime CreatedDateTime, NotificationSenderResponse NotificationSender);

public record TeamInviteNotificationResponse(string Content, bool IsRead, string ReceiverId, DateTime CreatedDateTime, NotificationSenderResponse NotificationSender, NotificationTeamResponse Team);

public record GetUserNotificationsResponse(List<InformationNotificationResponse> InformationNotifications, List<TeamInviteNotificationResponse> TeamInviteNotifications);
