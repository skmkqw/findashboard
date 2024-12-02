namespace ZBank.Contracts.Spaces.CreateSpace;

public record CreateSpaceResponse(Guid Id, Guid OwnerId, string Name, string Description);