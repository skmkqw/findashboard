using Mapster;
using ZBank.Contracts.Teams.GetTeams;
using ZBank.Domain.UserAggregate;

namespace ZBank.API.Mapping;

public class UserMappingConfigurations : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<User, GetTeamMemberResponse>()
            .Map(dest => dest.Id, src => src.Id.Value)
            .Map(dest => dest.FullName, src => $"{src.FirstName} {src.LastName}");
    }
}