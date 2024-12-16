using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ZBank.API.Interfaces;
using ZBank.Application.Spaces.Commands.CreateSpace;
using ZBank.Application.Spaces.Queries.GetSpace;
using ZBank.Contracts.Spaces.CreateSpace;
using ZBank.Contracts.Spaces.GetSpace;

namespace ZBank.API.Controllers;

[Route("api/personalSpaces")]
public class SpaceController : ApiController
{
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;
    private readonly ILogger<SpaceController> _logger;
    private readonly INotificationSender _notificationSender;

    public SpaceController(IMapper mapper,
        IMediator mediator,
        ILogger<SpaceController> logger,
        INotificationSender notificationSender)
    {
        _mapper = mapper;
        _mediator = mediator;
        _logger = logger;
        _notificationSender = notificationSender;
    }
    
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var ownerId = GetUserId();
        if (!ownerId.HasValue)
        {
            return UnauthorizedUserIdProblem();
        }
        
        var query = _mapper.Map<GetSpaceQuery>(ownerId);

        var getPersonalSpaceResult = await _mediator.Send(query);

        if (getPersonalSpaceResult.IsError)
        {
            _logger.LogInformation("Failed to fetch personal space for: {OwnerId}.\nErrors: {Errors}", ownerId, getPersonalSpaceResult.Errors);
            return Problem(getPersonalSpaceResult.Errors);
        }
        
        _logger.LogInformation("Successfully fetched personal space with id: {Id}", getPersonalSpaceResult.Value.Id.Value);
        return Ok(_mapper.Map<GetSpaceResponse>(getPersonalSpaceResult.Value));
    }
    
    [HttpPost]
    public async Task<IActionResult> Create(CreateSpaceRequest request)
    {
        var ownerId = GetUserId();
        if (!ownerId.HasValue)
        {
            return UnauthorizedUserIdProblem();
        }
        
        var command = _mapper.Map<CreateSpaceCommand>((ownerId, request));

        var createSpaceResult = await _mediator.Send(command);

        if (createSpaceResult.IsError)
        {
            _logger.LogInformation("Failed to create personal space for: {OwnerId}.\nErrors: {Errors}", ownerId, createSpaceResult.Errors);
            return Problem(createSpaceResult.Errors);
        }
        
        await _notificationSender.SendInformationNotification(createSpaceResult.Value.Notification);
        _logger.LogInformation("Successfully sent 'SpaceCreated' notification");
        
        _logger.LogInformation("Successfully created personal space with id: {Id}", createSpaceResult.Value.Result.Id.Value);
        return Ok(_mapper.Map<CreateSpaceResponse>(createSpaceResult.Value.Result));
    }
}