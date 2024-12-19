using MapsterMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ZBank.API.Interfaces;
using ZBank.Application.Profiles.Commands.CreateProfile;
using ZBank.Application.Wallets.Commands.CreateWallet;
using ZBank.Contracts.Profiles.CreateProfile;
using ZBank.Contracts.Wallets.CreateWallet;

namespace ZBank.API.Controllers;

[Route("api/wallets")]
public class WalletController : ApiController
{
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;
    private readonly INotificationSender _notificationSender;
    private readonly ILogger<WalletController> _logger;

    public WalletController(IMapper mapper,
        IMediator mediator,
        INotificationSender notificationSender,
        ILogger<WalletController> logger)
    {
        _mapper = mapper;
        _mediator = mediator;
        _logger = logger;
        _notificationSender = notificationSender;
    }
    
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateWalletRequest request)
    {
        var ownerId = GetUserId();
        if (!ownerId.HasValue)
        {
            return UnauthorizedUserIdProblem();
        }
        
        var command = _mapper.Map<CreateWalletCommand>((request, ownerId));
        
        var createWalletResult = await _mediator.Send(command);
        
        if (createWalletResult.IsError)
        {
            _logger.LogInformation("Failed to create wallet for: {OwnerId}.\nErrors: {Errors}", ownerId, createWalletResult.Errors);
            return Problem(createWalletResult.Errors);
        }
        
        await _notificationSender.SendInformationNotification(createWalletResult.Value.Notification);
        _logger.LogInformation("Successfully sent 'WalletCreated' notification");
        
        _logger.LogInformation("Successfully created wallet with id: {Id}", createWalletResult.Value.Result.Id.Value);
        return Ok(_mapper.Map<CreateWalletResponse>(createWalletResult.Value.Result));
    }
}