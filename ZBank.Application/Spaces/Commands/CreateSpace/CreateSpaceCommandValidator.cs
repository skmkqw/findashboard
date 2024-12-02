using FluentValidation;

namespace ZBank.Application.Spaces.Commands.CreateSpace;

public class CreateSpaceCommandValidator : AbstractValidator<CreateSpaceCommand>
{
    public CreateSpaceCommandValidator()
    {
        RuleFor(x => x.OwnerId.Value)
            .NotEmpty()
            .WithMessage("Owner ID is required.");
        
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Personal space name is required.");
    }
}