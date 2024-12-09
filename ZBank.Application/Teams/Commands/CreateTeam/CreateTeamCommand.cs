using ErrorOr;
using MediatR;
using ZBank.Application.Common.Models;
using ZBank.Domain.NotificationAggregate;
using ZBank.Domain.TeamAggregate;
using ZBank.Domain.UserAggregate.ValueObjects;

namespace ZBank.Application.Teams.Commands.CreateTeam;

public record CreateTeamCommand(string Name, string? Description, UserId OwnerId) : IRequest<ErrorOr<WithNotificationResult<Team, InformationNotification>>>;