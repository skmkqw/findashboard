using ErrorOr;
using MediatR;
using ZBank.Domain.NotificationAggregate.ValueObjects;
using ZBank.Domain.UserAggregate.ValueObjects;

namespace ZBank.Application.Notifications.Commands.DeleteNotification;

public record DeleteNotificationCommand(UserId UserId, NotificationId NotificationId) : IRequest<ErrorOr<Unit>>;