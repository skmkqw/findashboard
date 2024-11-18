using ErrorOr;
using MediatR;
using ZBank.Domain.TeamAggregate;
using ZBank.Domain.UserAggregate.ValueObjects;

namespace ZBank.Application.Teams.Commands.CreateTeam;

public record CreateTeamCommand(string Name, string? Description, UserId OwnerId) : IRequest<ErrorOr<Team>>;