namespace ZBank.Contracts.Teams.GetTeams;

public record GetUserTeamResponse(Guid Id, string Name, string? Description);

public record GetUserTeamsResponse(List<GetUserTeamResponse> Teams, GetUserTeamResponse? PersonalSpace);