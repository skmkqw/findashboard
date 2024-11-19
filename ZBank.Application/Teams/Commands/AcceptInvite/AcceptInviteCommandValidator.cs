using FluentValidation;

namespace ZBank.Application.Teams.Commands.AcceptInvite;

public class AcceptInviteCommandValidator : AbstractValidator<AcceptInviteCommand>
{
    public AcceptInviteCommandValidator()
    {
        RuleFor(x => x.UserId.Value)
            .NotEmpty()
            .WithMessage("User ID is required.");
        
        RuleFor(x => x.NotificationId.Value)
            .NotEmpty()
            .WithMessage("Notification ID is required.");
    }
}