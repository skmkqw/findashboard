using ErrorOr;
using MediatR;
using ZBank.Domain.NotificationAggregate;
using ZBank.Domain.UserAggregate.ValueObjects;

namespace ZBank.Application.Notifications.Queries.GetUserNotifications;

public record GetUserNotificationsQuery(UserId UserId) : IRequest<ErrorOr<Dictionary<string, List<Notification>>>>;