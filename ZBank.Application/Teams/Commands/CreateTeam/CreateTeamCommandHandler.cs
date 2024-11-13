using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;
using ZBank.Application.Common.Interfaces.Persistance;
using ZBank.Domain.Common.Errors;
using ZBank.Domain.TeamAggregate;
using ZBank.Domain.UserAggregate;

namespace ZBank.Application.Teams.Commands.CreateTeam;

public class CreateTeamCommandHandler : IRequestHandler<CreateTeamCommand, ErrorOr<Team>>
{
    private readonly ILogger<CreateTeamCommandHandler> _logger;
    
    private readonly IUserRepository _userRepository;
    
    private readonly ITeamRepository _teamRepository;
    
    private readonly IUnitOfWork _unitOfWork;

    public CreateTeamCommandHandler(IUserRepository userRepository,
        IUnitOfWork unitOfWork,
        ITeamRepository teamRepository,
        ILogger<CreateTeamCommandHandler> logger)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _teamRepository = teamRepository;
        _logger = logger;
    }

    public async Task<ErrorOr<Team>> Handle(CreateTeamCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling team creation for emails: {Email}", String.Join(", ", request.MemberEmails));
        List<User> members = new();

        var owner = await _userRepository.FindByIdAsync(request.OwnerId);

        if (owner is null)
        {
            _logger.LogInformation("Team owner with id: {Id} not found", request.OwnerId);
            return Errors.User.NotFound;
        }
        
        members.Add(owner);
        
        foreach (var email in request.MemberEmails)
        {
            var member = await _userRepository.FindByEmailAsync(email);

            if (member is not null)
            {
                members.Add(member);
                continue;
            }
            // TODO Domain Errors for TEAM????
            _logger.LogInformation("Team member with email: {Email} not found", email);

            return Errors.User.NotFound;
        }
        
        var team = Team.Create(
            name: request.Name,
            description: request.Description,
            userIds: members.Select(m => m.Id).ToList()
        );
        
        _teamRepository.Add(team);
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        _logger.LogInformation("Successfully created team.");
        return team;
    }
}