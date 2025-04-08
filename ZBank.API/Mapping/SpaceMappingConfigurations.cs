using Mapster;
using ZBank.Application.Spaces.Commands.CreateSpace;
using ZBank.Application.Spaces.Queries.GetSpace;
using ZBank.Contracts.Spaces.CreateSpace;
using ZBank.Contracts.Spaces.GetSpace;
using ZBank.Contracts.Teams.GetTeams;
using ZBank.Domain.Common.Errors;
using ZBank.Domain.TeamAggregate;
using ZBank.Domain.UserAggregate.ValueObjects;

namespace ZBank.API.Mapping;

public class SpaceMappingConfigurations : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<(Guid? ownerId, CreateSpaceRequest request), CreateSpaceCommand>()
            .Map(dest => dest.OwnerId, src => UserId.Create(src.ownerId!.Value))
            .Map(dest => dest.Name, src => src.request.SpaceName);

        config.NewConfig<PersonalSpace, CreateSpaceResponse>()
            .Map(dest => dest.Id, src => src.Id.Value)
            .Map(dest => dest.OwnerId, src => src.OwnerId.Value)
            .Map(dest => dest, src => src);

        config.NewConfig<Guid?, GetSpaceQuery>()
            .Map(dest => dest.OwnerId, src => UserId.Create(src!.Value));
        
        config.NewConfig<PersonalSpace, GetSpaceResponse>()
            .Map(dest => dest.Id, src => src.Id.Value)
            .Map(dest => dest.OwnerId, src => src.OwnerId.Value)
            .Map(dest => dest.ProjectIds, src => src.ProjectIds.Select(u => u.Value).ToList())
            .Map(dest => dest.ActivityIds, src => src.ActivityIds.Select(u => u.Value).ToList())
            .Map(dest => dest, src => src);
        
        config.NewConfig<PersonalSpace, GetUserTeamResponse>()
            .Map(dest => dest.Id, src => src.Id.Value);
    }
}