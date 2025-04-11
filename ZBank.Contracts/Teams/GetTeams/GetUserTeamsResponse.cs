namespace ZBank.Contracts.Teams.GetTeams;

public record GetTeamMemberResponse(Guid Id, string FullName, string Email);

public record GetSpaceResponse(Guid Id, string Name, string? Description);

public record GetTeamResponse(Guid Id, string Name, string? Description, List<GetTeamMemberResponse> Members);

public record GetUserTeamsResponse(List<GetTeamResponse> Teams, GetSpaceResponse? PersonalSpace);