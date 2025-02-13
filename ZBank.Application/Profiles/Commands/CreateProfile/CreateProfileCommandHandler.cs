using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;
using ZBank.Application.Common.Interfaces.Persistance;
using ZBank.Application.Common.Models;
using ZBank.Application.Teams.Common;
using ZBank.Domain.Common.Errors;
using ZBank.Domain.Common.Models;
using ZBank.Domain.NotificationAggregate;
using ZBank.Domain.NotificationAggregate.Factories;
using ZBank.Domain.ProfileAggregate;
using ZBank.Domain.UserAggregate;

namespace ZBank.Application.Profiles.Commands.CreateProfile;

public class CreateProfileCommandHandler : IRequestHandler<CreateProfileCommand, ErrorOr<WithNotificationResult<Profile, InformationNotification>>>
{
    private readonly IUserRepository _userRepository;
    
    private readonly ITeamRepository _teamRepository;
    
    private readonly IProfileRepository _profileRepository;
    
    private readonly INotificationRepository _notificationRepository;
    
    private readonly IUnitOfWork _unitOfWork;
    
    private readonly ILogger<CreateProfileCommandHandler> _logger;

    public CreateProfileCommandHandler(IUserRepository userRepository,
        ITeamRepository teamRepository,
        IProfileRepository profileRepository,
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
    }

    public async Task<ErrorOr<WithNotificationResult<Profile, InformationNotification>>> Handle(CreateProfileCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling profile creation for: {OwnerId} and team id = {TeamId}", request.OwnerId.Value, request.TeamId.Value);

        var member = await _userRepository.FindByIdAsync(request.OwnerId);

        if (member is null)
        {
            _logger.LogInformation("User with id: {Id} not found", request.OwnerId.Value);
            return Errors.User.IdNotFound(request.OwnerId);
        }
        
        var teamValidationDetails = await _teamRepository.GetTeamValidationDetailsAsync(request.TeamId, member);

        if (teamValidationDetails is null)
        {
            _logger.LogInformation("Team or personal space with id: {Id} not found", request.TeamId.Value);
            return Errors.Team.NotFound;
        }
        
        (TeamBase teamOrSpace, _) = teamValidationDetails.GetEntities();

        var createProfileValidationResult = ValidateCreateProfile(teamValidationDetails);
        
        if (createProfileValidationResult.IsError)
            return createProfileValidationResult.Errors;
        
        var profile = CreateProfile(request, member, teamOrSpace);
        _logger.LogInformation("Successfully created a profile.");

        var profileCreatedNotification = CreateProfileCreatedNotification(member, profile);
        _logger.LogInformation("'ProfileCreated' notification created");
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return new WithNotificationResult<Profile, InformationNotification>(profile, profileCreatedNotification);
    }

    private Profile CreateProfile(CreateProfileCommand request, User owner, TeamBase team)
    {
        var profile = Profile.Create(request.Name, request.OwnerId, request.TeamId);
        
        owner.AddProfileId(profile.Id);
        
        team.AddProfile(profile.Id);
        
        _profileRepository.Add(profile);
        
        return profile;
    }

    private ErrorOr<Success> ValidateCreateProfile(TeamValidationDetails teamValidationDetails)
    {
        if (teamValidationDetails.HasAccess)
        {
            _logger.LogInformation("Access to team with Id: {TeamId} was denied to user with Id: {UserId}", teamValidationDetails.TeamId.Value, teamValidationDetails.TeamId.Value);
            return Errors.Team.AccessDenied;
        }

        return Result.Success;
    }

    private InformationNotification CreateProfileCreatedNotification(User profileCreator, Profile profile)
    {
        var notification = NotificationFactory.CreateProfileCreatedNotification(profileCreator, profile);
        
        _notificationRepository.AddNotification(notification);
        
        profileCreator.AddNotificationId(notification.Id);
        
        return notification;
    }
}