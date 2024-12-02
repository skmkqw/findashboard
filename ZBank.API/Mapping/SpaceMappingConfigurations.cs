using Mapster;
using ZBank.Application.Users.Commands.CreateSpace;
using ZBank.Contracts.Spaces.CreateSpace;
using ZBank.Domain.TeamAggregate;
using ZBank.Domain.TeamAggregate.ValueObjects;

namespace ZBank.API.Mapping;

public class SpaceMappingConfigurations : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<(Guid? ownerId, CreateSpaceRequest request), CreateSpaceCommand>()
            .Map(dest => dest.OwnerId, src => TeamId.Create(src.ownerId!.Value))
            .Map(dest => dest.Name, src => src.request.SpaceName);

        config.NewConfig<PersonalSpace, CreateSpaceResponse>()
            .Map(dest => dest.OwnerId, src => src.OwnerId.Value)
            .Map(dest => dest, src => src);
    }
}