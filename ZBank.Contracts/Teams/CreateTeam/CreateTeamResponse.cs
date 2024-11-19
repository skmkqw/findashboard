namespace ZBank.Contracts.Teams.CreateTeam;

//TODO Include team id in response
public record CreateTeamResponse(string Name, string Description, List<string> MemberIds);