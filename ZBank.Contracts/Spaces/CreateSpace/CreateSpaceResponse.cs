namespace ZBank.Contracts.Spaces.CreateSpace;

public record CreateSpaceResponse(Guid OwnerId, string Name, string Description);