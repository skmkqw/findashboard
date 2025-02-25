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

namespace ZBank.Application.Spaces.Commands.CreateSpace;

public class CreateSpaceCommandHandler : IRequestHandler<CreateSpaceCommand, ErrorOr<WithNotificationResult<PersonalSpace, InformationNotification>>>
{
    private readonly IUserRepository _userRepository;
    
    private readonly ITeamRepository _teamRepository;
    
    private readonly INotificationRepository _notificationRepository;
    
    private readonly ILogger<CreateSpaceCommandHandler> _logger;
    
    private readonly IUnitOfWork _unitOfWork;

    public CreateSpaceCommandHandler(IUserRepository userRepository,
        ITeamRepository teamRepository,
        INotificationRepository notificationRepository,
        ILogger<CreateSpaceCommandHandler> logger,
        IUnitOfWork unitOfWork)
    {
        _userRepository = userRepository;
        _teamRepository = teamRepository;
        _notificationRepository = notificationRepository;
        _logger = logger;
        _unitOfWork = unitOfWork;
    }

    public async Task<ErrorOr<WithNotificationResult<PersonalSpace, InformationNotification>>> Handle(CreateSpaceCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling creating space command for user with id: {UserId}", request.OwnerId.Value);
        
        var owner = await _userRepository.FindByIdAsync(request.OwnerId);

        if (owner is null)
        {
            _logger.LogInformation("User with id: {UserId} not found or does not exist", request.OwnerId.Value);
            return Errors.User.IdNotFound(request.OwnerId);
        }
        
        if (ValidateCreateSpace(owner) is var validationResult && validationResult.IsError)
            return validationResult.Errors;
        
        var space = CreatePersonalSpace(request, owner);
        _logger.LogInformation("Successfully created personal space");
        
        var spaceCreatedNotification = CreateSpaceCreatedNotification(owner, space);
        _logger.LogInformation("'SpaceCreated' notification created");
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new WithNotificationResult<PersonalSpace, InformationNotification>(space, spaceCreatedNotification);
    }

    private PersonalSpace CreatePersonalSpace(CreateSpaceCommand request, User owner)
    {
        var space = PersonalSpace.Create(
            name: request.Name, 
            description: "Personal space only for you",
            ownerId: request.OwnerId
        );
        
        _teamRepository.AddSpace(space);
        owner.AssignPersonalSpaceId(space.Id);
        
        return space;
    }

    private ErrorOr<Success> ValidateCreateSpace(User owner)
    {
        if (owner.PersonalSpaceId is not null)
        {
            _logger.LogInformation("User with id: {UserId} already has a personal space", owner.Id.Value);
            return Errors.PersonalSpace.IsAlreadySet;
        }

        return Result.Success;
    }

    private InformationNotification CreateSpaceCreatedNotification(User owner, PersonalSpace space)
    {
        var spaceCreatedNotification = NotificationFactory.CreateSpaceCreatedNotification(owner, space);
        
        owner.AddNotificationId(spaceCreatedNotification.Id);
        
        _notificationRepository.AddNotification(spaceCreatedNotification);
        
        return spaceCreatedNotification;
    }
}