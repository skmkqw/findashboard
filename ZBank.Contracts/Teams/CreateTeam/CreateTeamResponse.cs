namespace ZBank.Contracts.Teams.CreateTeam;

public record CreateTeamResponse(string Id, string Name, string Description, List<string> MemberIds);