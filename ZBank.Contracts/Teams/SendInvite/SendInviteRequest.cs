namespace ZBank.Contracts.Teams.SendInvite;

public record SendInviteRequest(Guid SenderId, string SenderFullName, string ReceiverEmail, Guid TeamId, string TeamName);