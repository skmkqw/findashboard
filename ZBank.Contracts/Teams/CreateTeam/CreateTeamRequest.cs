namespace ZBank.Contracts.Teams.CreateTeam;

public record CreateTeamRequest(string Name, string? Description, List<string> MemberEmails);