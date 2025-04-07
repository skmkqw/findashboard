using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;
using ZBank.Application.Common.Interfaces.Persistance;
using ZBank.Application.Common.Models;
using ZBank.Domain.Common.Errors;
using ZBank.Domain.NotificationAggregate;
using ZBank.Domain.NotificationAggregate.Factories;
using ZBank.Domain.TeamAggregate;
using ZBank.Domain.UserAggregate;

namespace ZBank.Application.Teams.Commands.CreateTeam;

public class CreateTeamCommandHandler : IRequestHandler<CreateTeamCommand, ErrorOr<WithNotificationResult<Team, InformationNotification>>>
{
    private readonly ILogger<CreateTeamCommandHandler> _logger;
    
    private readonly IUserRepository _userRepository;
    
    private readonly ITeamRepository _teamRepository;
    
    private readonly INotificationRepository _notificationRepository;
    
    private readonly IUnitOfWork _unitOfWork;
    public CreateTeamCommandHandler(IUserRepository userRepository,
        IUnitOfWork unitOfWork,
        ITeamRepository teamRepository,
        ILogger<CreateTeamCommandHandler> logger,
        INotificationRepository notificationRepository) 
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _teamRepository = teamRepository;
        _logger = logger;
        _notificationRepository = notificationRepository;
    }

    public async Task<ErrorOr<WithNotificationResult<Team, InformationNotification>>> Handle(CreateTeamCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling team creation for: {OwnerId}", request.OwnerId.Value);

        var owner = await _userRepository.FindByIdAsync(request.OwnerId);

        if (owner is null)
        {
            _logger.LogInformation("Team owner with id: {Id} not found", request.OwnerId.Value);
            return Errors.User.IdNotFound(request.OwnerId);
        }

        var team = CreateTeam(request, owner);
        _logger.LogInformation("Team created");
        
        var teamCreatedNotification = CreateTeamCreatedNotification(owner, team);
        _logger.LogInformation("'TeamCreated' notification created");
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return new WithNotificationResult<Team, InformationNotification>(team, teamCreatedNotification);
    }

    private Team CreateTeam(CreateTeamCommand request, User owner)
    {
        var team = Team.Create(
            name: request.Name,
            description: request.Description,
            userIds: [owner.Id]
        );
        
        _teamRepository.AddTeam(team);
        
        return team;
    }
    
    private InformationNotification CreateTeamCreatedNotification(User teamCreator, Team team)
    {
        var notification = NotificationFactory.CreateTeamCreatedNotification(teamCreator.Id, team);
        
        _notificationRepository.AddNotification(notification);
        
        teamCreator.AddNotificationId(notification.Id);
        
        return notification;
    }
}