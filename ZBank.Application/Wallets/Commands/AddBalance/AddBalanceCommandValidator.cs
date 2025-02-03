using FluentValidation;

namespace ZBank.Application.Wallets.Commands.AddBalance;

public class AddBalanceCommandValidator : AbstractValidator<AddBalanceCommand>
{
    public AddBalanceCommandValidator()
    {
        RuleFor(x => x.Amount)
            .NotEmpty().WithMessage("Amount cannot be empty")
            .GreaterThan(0).WithMessage("Amount must be greater than 0");
        
        RuleFor(x => x.UserId.Value)
            .NotEmpty().WithMessage("Currency Id cannot be empty");

        RuleFor(x => x.CurrencyId.Value)
            .NotEmpty().WithMessage("Currency Id cannot be empty");
        
        RuleFor(x => x.WalletId.Value)
            .NotEmpty().WithMessage("Wallet Id cannot be empty");
    }
}