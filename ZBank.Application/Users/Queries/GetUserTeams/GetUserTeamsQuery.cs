using ErrorOr;
using MediatR;
using ZBank.Domain.TeamAggregate;
using ZBank.Domain.UserAggregate.ValueObjects;

namespace ZBank.Application.Users.Queries.GetUserTeams;

public record GetUserTeamsQuery(UserId UserId) : IRequest<ErrorOr<(List<GetTeamResult> Teams, PersonalSpace? PersonalSpace)>>;