using FluentValidation;

namespace ZBank.Application.Profiles.Commands.CreateProfile;

public class CreateProfileCommandValidator : AbstractValidator<CreateProfileCommand>
{
    public CreateProfileCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.")
            .Length(1, 100).WithMessage("Name must be between 1 and 100 characters.");
        
        RuleFor(x => x.OwnerId.Value)
            .NotEmpty().WithMessage("Owner ID is required.");
        
        RuleFor(x => x.TeamId.Value)
            .NotEmpty().WithMessage("Team ID is required.");
    }
}