using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;
using ZBank.Application.Common.Interfaces.Persistance;
using ZBank.Domain.Common.Errors;
using ZBank.Domain.NotificationAggregate.Factories;
using ZBank.Domain.TeamAggregate;
using ZBank.Domain.UserAggregate;

namespace ZBank.Application.Teams.Commands.CreateTeam;

public class CreateTeamCommandHandler : IRequestHandler<CreateTeamCommand, ErrorOr<Team>>
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

    public async Task<ErrorOr<Team>> Handle(CreateTeamCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling team creation for: {OwnerId}", request.OwnerId);

        var owner = await _userRepository.FindByIdAsync(request.OwnerId);

        if (owner is null)
        {
            _logger.LogInformation("Team owner with id: {Id} not found", request.OwnerId);
            return Errors.User.IdNotFound(request.OwnerId);
        }
        
        var team = Team.Create(
            name: request.Name,
            description: request.Description,
            userIds: [owner.Id]
        );
        
        _teamRepository.Add(team);
        
        SendTeamCreatedNotification(owner, team);
        _logger.LogInformation("'TeamCreated' notification sent");
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        _logger.LogInformation("Successfully created a team.");
        return team;
    }
    
    private void SendTeamCreatedNotification(User teamCreator, Team team)
    {
        var notification = NotificationFactory.CreateTeamCreatedNotification(teamCreator, team);
        
        _notificationRepository.AddInformationalNotification(notification);
        
        teamCreator.AddNotificationId(notification.Id);
    }
}