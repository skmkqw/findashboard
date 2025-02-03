using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;
using ZBank.Application.Common.Interfaces.Persistance;
using ZBank.Application.Common.Models;
using ZBank.Domain.Common.Errors;
using ZBank.Domain.NotificationAggregate;
using ZBank.Domain.NotificationAggregate.Factories;
using ZBank.Domain.UserAggregate;
using ZBank.Domain.WalletAggregate.Entities;

namespace ZBank.Application.Wallets.Commands.AddBalance;

public class AddBalanceCommandHandler : IRequestHandler<AddBalanceCommand, ErrorOr<WithNotificationResult<Balance, InformationNotification>>>
{
    private readonly IUserRepository _userRepository;
    
    private readonly IProfileRepository _profileRepository;
        
    private readonly IWalletRepository _walletRepository;
    
    private readonly ICurrencyRepository _currencyRepository;
    
    private readonly INotificationRepository _notificationRepository;
    
    private readonly IUnitOfWork _unitOfWork;
    
    private readonly ILogger<AddBalanceCommandHandler> _logger;

    public AddBalanceCommandHandler(IUserRepository userRepository,
        IProfileRepository profileRepository,
        IWalletRepository walletRepository,
        INotificationRepository notificationRepository,
        ICurrencyRepository currencyRepository,
        IUnitOfWork unitOfWork,
        ILogger<AddBalanceCommandHandler> logger)
    {
        _userRepository = userRepository;
        _profileRepository = profileRepository;
        _walletRepository = walletRepository;
        _notificationRepository = notificationRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
        _currencyRepository = currencyRepository;
    }

    public async Task<ErrorOr<WithNotificationResult<Balance, InformationNotification>>> Handle(AddBalanceCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling balance creation for: {UserId} on wallet id = {WalletId}", request.UserId.Value, request.WalletId.Value);

        var owner = await _userRepository.FindByIdAsync(request.UserId);
        if (owner is null)
        {
            _logger.LogInformation("User with id: {Id} not found", request.UserId.Value);
            return Errors.User.IdNotFound(request.UserId);
        }
        
        var wallet = await _walletRepository.GetById(request.WalletId);
        if (wallet is null)
        {
            _logger.LogInformation("Wallet with id: {Id} not found", request.WalletId.Value);
            return Errors.Wallet.NotFound;
        }
        
        var profile = await _profileRepository.GetByIdAsync(wallet.ProfileId);
        if (profile is null)
        {
            _logger.LogInformation("Profile with id: {Id} not found for wallet creation", wallet.ProfileId.Value);
            return Errors.Profile.NotFound;
        }

        if (profile.OwnerId != owner.Id)
        {
            _logger.LogInformation("User with id: {Id} is not the owner of profile with id: {ProfileId}", owner.Id.Value, profile.Id.Value);
            return Errors.Profile.AccessDenied;
        }
        
        var currency = await _currencyRepository.GetCurrencyById(request.CurrencyId);
        if (currency is null)
        {
            _logger.LogInformation("Currency with id: {Id} not found", request.CurrencyId.Value);
            return Errors.Currency.NotFound;
        }
        
        var balance = Balance.Create(currency.Id, request.Amount);
        
        wallet.AddBalance(balance);
        
        var walletCreatedNotification = CreateBalanceAddedNotification(owner, balance, wallet.Address);
        _logger.LogInformation("'BalanceAdded' notification created");
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        _logger.LogInformation("Successfully created a balance.");
        return new WithNotificationResult<Balance, InformationNotification>(balance, walletCreatedNotification);
    }
    
    private InformationNotification CreateBalanceAddedNotification(User balanceCreator, Balance balance, string walletAddress)
    {
        var notification = NotificationFactory.CreateBalanceAddedNotification(balanceCreator, balance, walletAddress);
        
        _notificationRepository.AddNotification(notification);
        
        balanceCreator.AddNotificationId(notification.Id);
        
        return notification;
    } 
}