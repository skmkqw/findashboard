using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;
using ZBank.Application.Common.Interfaces.Persistance;
using ZBank.Domain.Common.Errors;
using ZBank.Domain.TeamAggregate;
using ZBank.Domain.UserAggregate;

namespace ZBank.Application.Users.Queries.GetUserTeams;

public class GetUserTeamsQueryHandler : IRequestHandler<GetUserTeamsQuery, ErrorOr<(List<Team> Teams, PersonalSpace? PersonalSpace)>>
{
    private readonly IUserRepository _userRepository;
    
    private readonly ITeamRepository _teamRepository;
    
    private readonly ILogger<GetUserTeamsQueryHandler> _logger;

    public GetUserTeamsQueryHandler(IUserRepository userRepository,
        ITeamRepository teamRepository,
        ILogger<GetUserTeamsQueryHandler> logger)
    {
        _userRepository = userRepository;
        _teamRepository = teamRepository;
        _logger = logger;
    }
    
    public async Task<ErrorOr<(List<Team> Teams, PersonalSpace? PersonalSpace)>> Handle(GetUserTeamsQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling GetTeams query for user with id: {UserId}", request.UserId.Value);
        
        var user = await _userRepository.FindByIdAsync(request.UserId);

        if (user is null)
        {
            _logger.LogInformation("User with id: {UserId} not found or does not exist", request.UserId.Value);
            return Errors.User.IdNotFound(request.UserId);
        }
        
        var teams = await GetUserTeams(user);
        _logger.LogInformation("Successfully fetched user teams");
        
        var space = await GetUserPersonalSpace(user);
        _logger.LogInformation("Successfully fetched personal space");

        return (teams, space);
    }

    private async Task<List<Team>> GetUserTeams(User user)
    {
        var teams = new List<Team>();

        foreach (var teamId in user.TeamIds)
        {
            var team = await _teamRepository.GetTeamByIdAsync(teamId);
            
            if (team is not null)
                teams.Add(team);
        }
        
        return teams;
    }

    private async Task<PersonalSpace?> GetUserPersonalSpace(User user) => 
        user.PersonalSpaceId is not null ? await _teamRepository.GetSpaceByIdAsync(user.PersonalSpaceId!) : null;
}