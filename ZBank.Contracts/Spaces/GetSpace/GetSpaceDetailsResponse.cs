namespace ZBank.Contracts.Spaces.GetSpace;

public record GetSpaceDetailsResponse(Guid Id, Guid OwnerId, string Name, string Description, List<Guid> ProjectIds, List<Guid> ActivityIds);