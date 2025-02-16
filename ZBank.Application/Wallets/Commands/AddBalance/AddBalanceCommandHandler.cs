using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;
using ZBank.Application.Common.Interfaces.Persistance;
using ZBank.Application.Common.Models;
using ZBank.Application.Common.Models.Validation;
using ZBank.Domain.Common.Errors;
using ZBank.Domain.CurrencyAggregate;
using ZBank.Domain.NotificationAggregate;
using ZBank.Domain.NotificationAggregate.Factories;
using ZBank.Domain.UserAggregate;
using ZBank.Domain.UserAggregate.ValueObjects;
using ZBank.Domain.WalletAggregate;
using ZBank.Domain.WalletAggregate.Entities;

namespace ZBank.Application.Wallets.Commands.AddBalance;

public class AddBalanceCommandHandler : IRequestHandler<AddBalanceCommand, ErrorOr<WithNotificationResult<Balance, InformationNotification>>>
{
    private readonly IUserRepository _userRepository;
    
    private readonly IWalletRepository _walletRepository;
    
    private readonly ICurrencyRepository _currencyRepository;
    
    private readonly INotificationRepository _notificationRepository;
    
    private readonly IUnitOfWork _unitOfWork;
    
    private readonly ILogger<AddBalanceCommandHandler> _logger;

    public AddBalanceCommandHandler(IUserRepository userRepository,
        IWalletRepository walletRepository,
        INotificationRepository notificationRepository,
        ICurrencyRepository currencyRepository,
        IUnitOfWork unitOfWork,
        ILogger<AddBalanceCommandHandler> logger)
    {
        _userRepository = userRepository;
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
        
        var currency = await _currencyRepository.GetCurrencyById(request.CurrencyId);
        if (currency is null)
        {
            _logger.LogInformation("Currency with id: {Id} not found", request.CurrencyId.Value);
            return Errors.Currency.NotFound;
        }
        
        var walletValidationDetails = await _walletRepository.GetWalletValidationDetails(request.WalletId);

        if (walletValidationDetails is null)
        {
            _logger.LogInformation("Wallet with id: {Id} not found", request.WalletId.Value);
            return Errors.Wallet.NotFound;
        }

        var wallet = walletValidationDetails.GetWallet();
        
        var validateAddBalanceResult = ValidateAddBalance(walletValidationDetails, owner.Id);
        if (validateAddBalanceResult.IsError)
            return validateAddBalanceResult.Errors;

        var balance = AddBalance(wallet, currency, request.Amount);
        _logger.LogInformation("Successfully created a balance.");
        
        var walletCreatedNotification = CreateBalanceAddedNotification(owner, balance, wallet.Address);
        _logger.LogInformation("'BalanceAdded' notification created");
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return new WithNotificationResult<Balance, InformationNotification>(balance, walletCreatedNotification);
    }

    private ErrorOr<Success> ValidateAddBalance(WalletValidationDetails walletValidationDetails, UserId ownerId)
    {
        if (!walletValidationDetails.HasAccess(ownerId))
        {
            _logger.LogInformation("User with id: {Id} is not the owner of profile with id: {ProfileId}", ownerId.Value, walletValidationDetails.ProfileId.Value);
            return Errors.Profile.AccessDenied;
        }

        return Result.Success;
    }

    private Balance AddBalance(Wallet wallet, Currency currency, decimal amount)
    {
        var balance = Balance.Create(currency.Id, amount);
        
        wallet.AddBalance(balance);

        return balance;
    }
    
    private InformationNotification CreateBalanceAddedNotification(User balanceCreator, Balance balance, string walletAddress)
    {
        var notification = NotificationFactory.CreateBalanceAddedNotification(balanceCreator, balance, walletAddress);
        
        _notificationRepository.AddNotification(notification);
        
        balanceCreator.AddNotificationId(notification.Id);
        
        return notification;
    } 
}