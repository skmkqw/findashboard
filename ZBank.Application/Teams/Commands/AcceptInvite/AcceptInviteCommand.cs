using ErrorOr;
using MediatR;
using ZBank.Application.Common.Models;
using ZBank.Domain.NotificationAggregate;
using ZBank.Domain.NotificationAggregate.ValueObjects;
using ZBank.Domain.UserAggregate.ValueObjects;

namespace ZBank.Application.Teams.Commands.AcceptInvite;

public record AcceptInviteCommand(UserId UserId, NotificationId NotificationId) : IRequest<ErrorOr<WithNotificationResult<Unit, InformationNotification>>>;