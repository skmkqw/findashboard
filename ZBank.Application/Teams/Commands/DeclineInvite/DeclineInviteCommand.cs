using ErrorOr;
using MediatR;
using ZBank.Domain.NotificationAggregate.ValueObjects;
using ZBank.Domain.UserAggregate.ValueObjects;

namespace ZBank.Application.Teams.Commands.DeclineInvite;

public record DeclineInviteCommand(UserId UserId, NotificationId NotificationId) : IRequest<ErrorOr<Unit>>;