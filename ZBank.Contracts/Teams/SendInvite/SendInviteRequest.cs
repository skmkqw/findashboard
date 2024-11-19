namespace ZBank.Contracts.Teams.SendInvite;

public record SendInviteRequest(string SenderFullName, string ReceiverEmail, Guid TeamId, string TeamName);