using FluentValidation;

namespace ZBank.Application.Teams.Commands.DeclineInvite;

public class DeclineInviteCommandValidator : AbstractValidator<DeclineInviteCommand>
{
    public DeclineInviteCommandValidator()
    {
        RuleFor(x => x.UserId.Value)
            .NotEmpty()
            .WithMessage("User ID is required.");
        
        RuleFor(x => x.NotificationId.Value)
            .NotEmpty()
            .WithMessage("Notification ID is required.");
    }
}