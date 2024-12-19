using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ZBank.API.Interfaces;
using ZBank.Application.Profiles.Commands.CreateProfile;
using ZBank.Contracts.Profiles.CreateProfile;

namespace ZBank.API.Controllers;

[Route("api/profiles")]
public class ProfileController : ApiController
{
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;
    private readonly INotificationSender _notificationSender;
    private readonly ILogger<ProfileController> _logger;

    public ProfileController(IMapper mapper,
        IMediator mediator,
        INotificationSender notificationSender,
        ILogger<ProfileController> logger)
    {
        _mapper = mapper;
        _mediator = mediator;
        _logger = logger;
        _notificationSender = notificationSender;
    }
    
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateProfileRequest request)
    {
        var ownerId = GetUserId();
        if (!ownerId.HasValue)
        {
            return UnauthorizedUserIdProblem();
        }
        
        var command = _mapper.Map<CreateProfileCommand>((request, ownerId));
        
        var createProfileResult = await _mediator.Send(command);
        
        if (createProfileResult.IsError)
        {
            _logger.LogInformation("Failed to create profile for: {OwnerId}.\nErrors: {Errors}", ownerId, createProfileResult.Errors);
            return Problem(createProfileResult.Errors);
        }
        
        await _notificationSender.SendInformationNotification(createProfileResult.Value.Notification);
        _logger.LogInformation("Successfully sent 'ProfileCreated' notification");
        
        _logger.LogInformation("Successfully created profile with id: {Id}", createProfileResult.Value.Result.Id.Value);
        return Ok(_mapper.Map<CreateProfileResponse>(createProfileResult.Value));
    }
}