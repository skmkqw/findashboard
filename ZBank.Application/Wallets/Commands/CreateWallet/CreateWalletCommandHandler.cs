using MediatR;
using ErrorOr;
using Microsoft.Extensions.Logging;
using ZBank.Application.Common.Interfaces.Persistance;
using ZBank.Application.Common.Models;
using ZBank.Domain.Common.Errors;
using ZBank.Domain.NotificationAggregate;
using ZBank.Domain.NotificationAggregate.Factories;
using ZBank.Domain.UserAggregate;
using ZBank.Domain.WalletAggregate;
using ZBank.Domain.WalletAggregate.ValueObjects;

namespace ZBank.Application.Wallets.Commands.CreateWallet;

public class CreateWalletCommandHandler : IRequestHandler<CreateWalletCommand, ErrorOr<WithNotificationResult<Wallet, InformationNotification>>>
{
    private readonly IUserRepository _userRepository;
    
    private readonly IProfileRepository _profileRepository;
        
    private readonly IWalletRepository _walletRepository;
    
    private readonly INotificationRepository _notificationRepository;
    
    private readonly IUnitOfWork _unitOfWork;
    
    private readonly ILogger<CreateWalletCommandHandler> _logger;
    
    public CreateWalletCommandHandler(IUserRepository userRepository,
        IUnitOfWork unitOfWork,
        IWalletRepository walletRepository,
        IProfileRepository profileRepository, 
        INotificationRepository notificationRepository,
        ILogger<CreateWalletCommandHandler> logger) 
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _walletRepository = walletRepository;
        _logger = logger;
        _notificationRepository = notificationRepository;
        _profileRepository = profileRepository;
    }
    
    public async Task<ErrorOr<WithNotificationResult<Wallet, InformationNotification>>> Handle(CreateWalletCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling wallet creation for: {OwnerId} on profile id = {ProfileId}", request.OwnerId.Value, request.ProfileId.Value);

        var owner = await _userRepository.FindByIdAsync(request.OwnerId);

        if (owner is null)
        {
            _logger.LogInformation("User with id: {Id} not found", request.OwnerId.Value);
            return Errors.User.IdNotFound(request.OwnerId);
        }
        
        var profile = await _profileRepository.GetByIdAsync(request.ProfileId);
        if (profile is null)
        {
            _logger.LogInformation("Profile with id: {Id} not found for wallet creation", request.ProfileId.Value);
            return Errors.Profile.NotFound;
        }

        if (profile.OwnerId != owner.Id)
        {
            _logger.LogInformation("User with id: {Id} is not the owner of profile with id: {ProfileId}", owner.Id.Value, profile.Id.Value);
            return Errors.Profile.AccessDenied;
        }

        if (!Enum.TryParse<WalletType>(request.Type, ignoreCase: true, out var walletType))
        {
            _logger.LogInformation("Invalid wallet type: {Type} for wallet creation", request.Type);
            return Errors.Wallet.InvalidType;
        }

        var wallet = Wallet.Create(
            address: request.Address,
            type: walletType,
            profileId: request.ProfileId
        );
        
        _walletRepository.Add(wallet);
        
        profile.AddWallet(wallet.Id);
        
        var walletCreatedNotification = CreateWalletCreatedNotification(owner, wallet);
        _logger.LogInformation("'WalletCreated' notification created");
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        _logger.LogInformation("Successfully created a wallet.");
        return new WithNotificationResult<Wallet, InformationNotification>(wallet, walletCreatedNotification);
    }
    
    private InformationNotification CreateWalletCreatedNotification(User walletCreator, Wallet wallet)
    {
        var notification = NotificationFactory.CreateWalletCreatedNotification(walletCreator, wallet);
        
        _notificationRepository.AddNotification(notification);
        
        walletCreator.AddNotificationId(notification.Id);
        
        return notification;
    } 
}