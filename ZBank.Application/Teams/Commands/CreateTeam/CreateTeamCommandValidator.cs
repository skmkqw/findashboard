using FluentValidation;

namespace ZBank.Application.Teams.Commands.CreateTeam;

public class CreateTeamCommandValidator : AbstractValidator<CreateTeamCommand>
{
    public CreateTeamCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Name is required");

        RuleFor(x => x.Description)
            .MaximumLength(100)
            .WithMessage("Description must be less than 100 characters");
        
        RuleFor(x => x.MemberEmails)
            .NotEmpty()
            .WithMessage("At least one member is required")
            .ForEach(memberEmail =>
            {
                memberEmail
                    .NotEmpty()
                    .WithMessage("Member email cannot be empty")
                    .EmailAddress()
                    .WithMessage("{PropertyValue} is not a valid email address");
            });
    }
}