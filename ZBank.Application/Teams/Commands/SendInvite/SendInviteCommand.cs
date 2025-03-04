using ErrorOr;
using MediatR;
using ZBank.Application.Common.Models;
using ZBank.Domain.NotificationAggregate;
using ZBank.Domain.TeamAggregate.ValueObjects;
using ZBank.Domain.UserAggregate.ValueObjects;

namespace ZBank.Application.Teams.Commands.SendInvite;

public record SendInviteCommand(UserId SenderId, string ReceiverEmail, TeamId TeamId) : IRequest<ErrorOr<WithNotificationResult<TeamInviteNotification, InformationNotification>>>;