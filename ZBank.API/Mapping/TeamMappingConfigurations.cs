using Mapster;
using ZBank.Application.Teams.Commands.AcceptInvite;
using ZBank.Application.Teams.Commands.CreateTeam;
using ZBank.Application.Teams.Commands.DeclineInvite;
using ZBank.Application.Teams.Commands.SendInvite;
using ZBank.Application.Users.Queries.GetUserTeams;
using ZBank.Contracts.Spaces.GetSpace;
using ZBank.Contracts.Teams.CreateTeam;
using ZBank.Contracts.Teams.GetTeams;
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
        config.NewConfig<(SendInviteRequest request, Guid? senderId), SendInviteCommand>()
            .Map(dest => dest.SenderId, src => UserId.Create(src.senderId!.Value))
            .Map(dest => dest.TeamId, src => TeamId.Create(src.request.TeamId))
            .Map(dest => dest, src => src.request);

        config.NewConfig<(Guid? userId, Guid inviteId), AcceptInviteCommand>()
            .Map(dest => dest.UserId, src => UserId.Create(src.userId!.Value))
            .Map(dest => dest.NotificationId, src => NotificationId.Create(src.inviteId));
        
        config.NewConfig<(Guid? userId, Guid inviteId), DeclineInviteCommand>()
            .Map(dest => dest.UserId, src => UserId.Create(src.userId!.Value))
            .Map(dest => dest.NotificationId, src => NotificationId.Create(src.inviteId));
        
        config.NewConfig<(CreateTeamRequest request, Guid? ownerId), CreateTeamCommand>()
            .Map(dest => dest.OwnerId, src => UserId.Create(src.ownerId!.Value))
            .Map(dest => dest, src => src.request);

        config.NewConfig<Team, CreateTeamResponse>()
            .Map(dest => dest.Id, src => src.Id.Value.ToString())
            .Map(dest => dest.MemberIds, src => src.UserIds.Select(u => u.Value.ToString()).ToList());

        config.NewConfig<Guid, GetUserTeamsQuery>()
            .Map(dest => dest.UserId, src => UserId.Create(src));
        
        config.NewConfig<Team, GetTeamResponse>()
            .Map(dest => dest.Id, src => src.Id.Value);
        
        config.NewConfig<GetTeamResult, GetTeamResponse>()
            .Map(dest => dest.Id, src => src.Team.Id.Value)
            .Map(dest => dest.Members, src => src.Members.Adapt<List<GetTeamMemberResponse>>())
            .Map(dest => dest, src => src.Team);
        
        config.NewConfig<(List<GetTeamResult> Teams, PersonalSpace? PersonalSpace), GetUserTeamsResponse>()
            .Map(dest => dest.Teams, src => src.Teams.Adapt<List<GetTeamResponse>>().ToList())
            .Map(dest => dest.PersonalSpace, src => src.PersonalSpace.Adapt<GetTeamResponse>());
    }
}