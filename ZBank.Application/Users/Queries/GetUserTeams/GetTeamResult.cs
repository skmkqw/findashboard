using ZBank.Domain.TeamAggregate;
using ZBank.Domain.UserAggregate;

namespace ZBank.Application.Users.Queries.GetUserTeams;

public record GetTeamResult(Team Team, List<User> Members);