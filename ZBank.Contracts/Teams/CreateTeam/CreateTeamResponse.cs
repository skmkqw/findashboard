namespace ZBank.Contracts.Teams.CreateTeam;

public record CreateTeamResponse(string Name, string Description, List<string> MemberIds);