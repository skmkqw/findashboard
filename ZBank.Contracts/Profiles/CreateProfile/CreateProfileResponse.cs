namespace ZBank.Contracts.Profiles.CreateProfile;

public record CreateProfileResponse(Guid Id, string Name, Guid TeamId, Guid OwnerId);