using ErrorOr;
using MediatR;
using ZBank.Application.Common.Models;
using ZBank.Domain.NotificationAggregate;
using ZBank.Domain.ProfileAggregate.ValueObjects;
using ZBank.Domain.UserAggregate.ValueObjects;
using ZBank.Domain.WalletAggregate;
using ZBank.Domain.WalletAggregate.ValueObjects;

namespace ZBank.Application.Wallets.Commands.CreateWallet;

public record CreateWalletCommand(string Address, WalletType Type, UserId UserId, ProfileId ProfileId) : IRequest<ErrorOr<WithNotificationResult<Wallet, InformationNotification>>>;