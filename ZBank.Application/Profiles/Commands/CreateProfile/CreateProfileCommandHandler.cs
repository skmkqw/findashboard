using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;
using ZBank.Application.Common.Interfaces.Persistance;
using ZBank.Application.Common.Models;
using ZBank.Domain.Common.Errors;
using ZBank.Domain.Common.Models;
using ZBank.Domain.NotificationAggregate;
using ZBank.Domain.NotificationAggregate.Factories;
using ZBank.Domain.ProfileAggregate;
using ZBank.Domain.TeamAggregate;
using ZBank.Domain.UserAggregate;

namespace ZBank.Application.Profiles.Commands.CreateProfile;

public class CreateProfileCommandHandler : IRequestHandler<CreateProfileCommand, ErrorOr<WithNotificationResult<Profile, InformationNotification>>>
{
    private readonly IUserRepository _userRepository;
    
    private readonly ITeamRepository _teamRepository;
    
    private readonly ISpaceRepository _spaceRepository; 
    
    private readonly IProfileRepository _profileRepository;
    
    private readonly INotificationRepository _notificationRepository;
    
    private readonly IUnitOfWork _unitOfWork;
    
    private readonly ILogger<CreateProfileCommandHandler> _logger;

    public CreateProfileCommandHandler(IUserRepository userRepository,
        ITeamRepository teamRepository,
        IProfileRepository profileRepository,
        ISpaceRepository spaceRepository,
        INotificationRepository notificationRepository,
        IUnitOfWork unitOfWork,
        ILogger<CreateProfileCommandHandler> logger)
    {
        _userRepository = userRepository;
        _teamRepository = teamRepository;
        _profileRepository = profileRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
        _notificationRepository = notificationRepository;
        _spaceRepository = spaceRepository;
    }

    public async Task<ErrorOr<WithNotificationResult<Profile, InformationNotification>>> Handle(CreateProfileCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling profile creation for: {OwnerId} and team id = {TeamId}", request.OwnerId.Value, request.TeamId.Value);

        var owner = await _userRepository.FindByIdAsync(request.OwnerId);

        if (owner is null)
        {
            _logger.LogInformation("User with id: {Id} not found", request.OwnerId.Value);
            return Errors.User.IdNotFound(request.OwnerId);
        }
        
        TeamBase? teamOrSpace = await _teamRepository.GetByIdAsync(request.TeamId);

        if (teamOrSpace is null && owner.PersonalSpaceId is not null)
        {
            teamOrSpace = await _spaceRepository.GetByIdAsync(owner.PersonalSpaceId);

            if (teamOrSpace is null)
            {
                _logger.LogInformation("Neither a team nor personal space is available for user with id: {Id}", owner.Id.Value);
                return Errors.PersonalSpace.IsNotSet;
            }
        }
        else if (teamOrSpace is null && owner.PersonalSpaceId is null)
        {
            return Errors.Team.NotFound;
        }

        if (teamOrSpace is Team team && !team.UserIds.Contains(owner.Id))
        {
            _logger.LogInformation("User with id: {Id} is not a member of team with id: {TeamId}", owner.Id.Value, team.Id.Value);
            return Errors.Team.AccessDenied;
        }
        
        var profile = Profile.Create(request.Name, request.OwnerId, request.TeamId);
        
        owner.AddProfileId(profile.Id);
        
        teamOrSpace!.AddProfile(profile.Id);
        
        _profileRepository.Add(profile);
        
        var profileCreatedNotification = CreateProfileCreatedNotification(owner, profile);
        _logger.LogInformation("'ProfileCreated' notification created");
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        _logger.LogInformation("Successfully created a profile.");

        return new WithNotificationResult<Profile, InformationNotification>(profile, profileCreatedNotification);
    }

    private InformationNotification CreateProfileCreatedNotification(User profileCreator, Profile profile)
    {
        var notification = NotificationFactory.CreateProfileCreatedNotification(profileCreator, profile);
        
        _notificationRepository.AddNotification(notification);
        
        profileCreator.AddNotificationId(notification.Id);
        
        return notification;
    }
}