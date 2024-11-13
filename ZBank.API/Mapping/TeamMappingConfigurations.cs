using Mapster;
using ZBank.Application.Teams.Commands.CreateTeam;
using ZBank.Contracts.Teams.CreateTeam;
using ZBank.Domain.TeamAggregate;

namespace ZBank.API.Mapping;

public class TeamMappingConfigurations : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<CreateTeamRequest, CreateTeamCommand>();

        config.NewConfig<Team, CreateTeamResponse>()
            .Map(dest => dest.MemberIds, src => src.UserIds.Select(u => u.Value.ToString()).ToList());
    }
}