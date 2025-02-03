using Mapster;
using ZBank.Application.Profiles.Commands.CreateProfile;
using ZBank.Contracts.Profiles.CreateProfile;
using ZBank.Domain.ProfileAggregate;
using ZBank.Domain.TeamAggregate.ValueObjects;
using ZBank.Domain.UserAggregate.ValueObjects;

namespace ZBank.API.Mapping;

public class ProfileMappingConfigurations : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<(CreateProfileRequest request, Guid? ownerId), CreateProfileCommand>()
            .Map(dest => dest.OwnerId, src => UserId.Create(src.ownerId!.Value))
            .Map(dest => dest.TeamId, src => TeamId.Create(src.request.TeamId))
            .Map(dest => dest, src => src.request);
        
        config.NewConfig<Profile, CreateProfileResponse>()
            .Map(dest => dest.Id, src => src.Id.Value)
            .Map(dest => dest.OwnerId, src => src.OwnerId.Value)
            .Map(dest => dest.TeamId, src => src.TeamId.Value);
    }
}