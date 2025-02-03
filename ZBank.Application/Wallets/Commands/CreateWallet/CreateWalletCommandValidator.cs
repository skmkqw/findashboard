using FluentValidation;

namespace ZBank.Application.Wallets.Commands.CreateWallet;

public class CreateWalletCommandValidator : AbstractValidator<CreateWalletCommand>
{
    public CreateWalletCommandValidator()
    {
        RuleFor(x => x.Address)
            .NotEmpty().WithMessage("Address is required")
            .Length(20, 200).WithMessage("Address must be between 20 and 200 characters long");
        
        RuleFor(x => x.Type)
            .NotEmpty().WithMessage("Type is required");
        
        RuleFor(x => x.OwnerId.Value)
            .NotEmpty().WithMessage("Owner ID is required");
        
        RuleFor(x => x.ProfileId.Value)
            .NotEmpty().WithMessage("Profile ID is required");
    }
}