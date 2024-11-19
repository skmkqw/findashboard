namespace ZBank.Contracts.Teams.SendInvite;

public record SendInviteRequest(string ReceiverEmail, Guid TeamId);