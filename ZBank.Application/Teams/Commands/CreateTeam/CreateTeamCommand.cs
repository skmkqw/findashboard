using ErrorOr;
using MediatR;
using ZBank.Domain.TeamAggregate;

namespace ZBank.Application.Teams.Commands.CreateTeam;

public record CreateTeamCommand(string Name, string? Description, List<string> MemberEmails) : IRequest<ErrorOr<Team>>;