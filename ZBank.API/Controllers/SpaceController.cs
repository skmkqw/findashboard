using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ZBank.Application.Spaces.Commands.CreateSpace;
using ZBank.Contracts.Spaces.CreateSpace;

namespace ZBank.API.Controllers;

[Route("api/personalSpaces")]
public class SpaceController : ApiController
{
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;
    private readonly ILogger<SpaceController> _logger;

    public SpaceController(IMapper mapper, IMediator mediator, ILogger<SpaceController> logger)
    {
        _mapper = mapper;
        _mediator = mediator;
        _logger = logger;
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
        
        _logger.LogInformation("Successfully created personal space with id: {Id}", createSpaceResult.Value.Id.Value);
        return Ok(_mapper.Map<CreateSpaceResponse>(createSpaceResult.Value));
    }
}