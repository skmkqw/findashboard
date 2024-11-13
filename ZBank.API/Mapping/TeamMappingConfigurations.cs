using Mapster;
using ZBank.Application.Teams.Commands.CreateTeam;
using ZBank.Contracts.Teams.CreateTeam;
using ZBank.Domain.TeamAggregate;
using ZBank.Domain.UserAggregate.ValueObjects;

namespace ZBank.API.Mapping;

public class TeamMappingConfigurations : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<(CreateTeamRequest request, Guid? ownerId), CreateTeamCommand>()
            .Map(dest => dest.OwnerId, src => UserId.Create(src.ownerId!.Value))
            .Map(dest => dest, src => src.request);

        config.NewConfig<Team, CreateTeamResponse>()
            .Map(dest => dest.MemberIds, src => src.UserIds.Select(u => u.Value.ToString()).ToList());
    }
}