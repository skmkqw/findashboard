using MediatR;
using ErrorOr;
using Microsoft.Extensions.Logging;
using ZBank.Application.Common.Interfaces.Persistance;
using ZBank.Application.Common.Models;
using ZBank.Application.Common.Models.Validation;
using ZBank.Domain.Common.Errors;
using ZBank.Domain.NotificationAggregate;
using ZBank.Domain.NotificationAggregate.Factories;
using ZBank.Domain.ProfileAggregate;
using ZBank.Domain.UserAggregate;
using ZBank.Domain.WalletAggregate;
using ZBank.Domain.WalletAggregate.ValueObjects;

namespace ZBank.Application.Wallets.Commands.CreateWallet;

public class CreateWalletCommandHandler : IRequestHandler<CreateWalletCommand, ErrorOr<WithNotificationResult<Wallet, InformationNotification>>>
{
    private readonly IProfileRepository _profileRepository;
        
    private readonly IWalletRepository _walletRepository;
    
    private readonly INotificationRepository _notificationRepository;
    
    private readonly IUnitOfWork _unitOfWork;
    
    private readonly ILogger<CreateWalletCommandHandler> _logger;
    
    public CreateWalletCommandHandler(
        IUnitOfWork unitOfWork,
        IWalletRepository walletRepository,
        IProfileRepository profileRepository, 
        INotificationRepository notificationRepository,
        ILogger<CreateWalletCommandHandler> logger) 
    {
        _unitOfWork = unitOfWork;
        _walletRepository = walletRepository;
        _logger = logger;
        _notificationRepository = notificationRepository;
        _profileRepository = profileRepository;
    }
    
    public async Task<ErrorOr<WithNotificationResult<Wallet, InformationNotification>>> Handle(CreateWalletCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling wallet creation for: {OwnerId} on profile id = {ProfileId}", request.OwnerId.Value, request.ProfileId.Value);

        var profileValidationDetails = await _profileRepository.GetProfileValidationDetailsAsync(request.ProfileId);
        if (profileValidationDetails == null)
        {
            _logger.LogWarning("Profile with id {ProfileId} not found or does not exist", request.ProfileId);
            return Errors.Profile.NotFound;
        }
        
        var (profile, owner) = profileValidationDetails.GetEntities();

        var createWalletValidationResult = ValidateCreateWallet(request, profileValidationDetails);

        if (createWalletValidationResult.IsError)
            return createWalletValidationResult.Errors;
        
        var wallet = CreateWallet(request, Enum.Parse<WalletType>(request.Type), profile);
        _logger.LogInformation("Successfully created a wallet.");
        
        var walletCreatedNotification = CreateWalletCreatedNotification(owner, wallet);
        _logger.LogInformation("'WalletCreated' notification created");
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return new WithNotificationResult<Wallet, InformationNotification>(wallet, walletCreatedNotification);
    }

    private Wallet CreateWallet(CreateWalletCommand request, WalletType walletType, Profile profile)
    {
        var wallet = Wallet.Create(
            address: request.Address,
            type: walletType,
            profileId: request.ProfileId
        );
        
        _walletRepository.Add(wallet);
        
        profile.AddWallet(wallet.Id);
        
        return wallet;
    }

    private ErrorOr<Success> ValidateCreateWallet(CreateWalletCommand request, ProfileValidationDetails profileValidationDetails)
    {
        if (profileValidationDetails.OwnerId != request.OwnerId)
        {
            _logger.LogInformation("User with id: {Id} is not the owner of profile with id: {ProfileId}", profileValidationDetails.OwnerId.Value, profileValidationDetails.ProfileId.Value);
            return Errors.Profile.AccessDenied;
        }

        if (!Enum.TryParse<WalletType>(request.Type, ignoreCase: true, out _))
        {
            _logger.LogInformation("Invalid wallet type: {Type} for wallet creation", request.Type);
            return Errors.Wallet.InvalidType;
        }
        
        return Result.Success;
    }
    
    private InformationNotification CreateWalletCreatedNotification(User walletCreator, Wallet wallet)
    {
        var notification = NotificationFactory.CreateWalletCreatedNotification(walletCreator, wallet);
        
        _notificationRepository.AddNotification(notification);
        
        walletCreator.AddNotificationId(notification.Id);
        
        return notification;
    } 
}