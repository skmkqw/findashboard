using ErrorOr;
using MediatR;
using ZBank.Domain.NotificationAggregate.ValueObjects;
using ZBank.Domain.TeamAggregate.ValueObjects;

namespace ZBank.Application.Teams.Commands.SendInvite;

public record SendInviteCommand(NotificationSender Sender, string ReceiverEmail, TeamId TeamId, string TeamName) : IRequest<ErrorOr<Unit>>;