using Mapster;
using ZBank.Application.Teams.Commands.CreateTeam;
using ZBank.Application.Teams.Commands.SendInvite;
using ZBank.Contracts.Teams.CreateTeam;
using ZBank.Contracts.Teams.SendInvite;
using ZBank.Domain.NotificationAggregate.ValueObjects;
using ZBank.Domain.TeamAggregate;
using ZBank.Domain.TeamAggregate.ValueObjects;
using ZBank.Domain.UserAggregate.ValueObjects;

namespace ZBank.API.Mapping;

public class TeamMappingConfigurations : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<(SendInviteRequest request, UserId senderId), SendInviteCommand>()
            .Map(dest => dest.Sender, src => NotificationSender.Create(src.senderId, src.request.SenderFullName))
            .Map(dest => dest.TeamId, src => TeamId.Create(src.request.TeamId))
            .Map(dest => dest, src => src.request);
        
        config.NewConfig<(CreateTeamRequest request, Guid? ownerId), CreateTeamCommand>()
            .Map(dest => dest.OwnerId, src => UserId.Create(src.ownerId!.Value))
            .Map(dest => dest, src => src.request);

        config.NewConfig<Team, CreateTeamResponse>()
            .Map(dest => dest.MemberIds, src => src.UserIds.Select(u => u.Value.ToString()).ToList());
    }
}