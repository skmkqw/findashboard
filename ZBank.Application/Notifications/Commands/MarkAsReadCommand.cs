using ErrorOr;
using MediatR;
using ZBank.Domain.NotificationAggregate.ValueObjects;
using ZBank.Domain.UserAggregate.ValueObjects;

namespace ZBank.Application.Notifications.Commands;

public record MarkAsReadCommand(UserId UserId, NotificationId NotificationId) : IRequest<ErrorOr<Unit>>;