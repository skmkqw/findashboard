using ErrorOr;
using MediatR;
using ZBank.Application.Common.Models;
using ZBank.Domain.CurrencyAggregate.ValueObjects;
using ZBank.Domain.NotificationAggregate;
using ZBank.Domain.UserAggregate.ValueObjects;
using ZBank.Domain.WalletAggregate.Entities;
using ZBank.Domain.WalletAggregate.ValueObjects;

namespace ZBank.Application.Wallets.Commands.AddBalance;

public record AddBalanceCommand(UserId UserId, WalletId WalletId, CurrencyId Symbol, decimal Amount) : IRequest<ErrorOr<WithNotificationResult<Balance, InformationNotification>>>;