using FluentValidation;

namespace ZBank.Application.Teams.Commands.SendInvite;

public class SendInviteCommandValidator : AbstractValidator<SendInviteCommand>
{
    public SendInviteCommandValidator()
    {
        RuleFor(x => x.SenderId.Value)
            .NotEmpty()
            .WithMessage("Sender ID is required.");
        
        RuleFor(x => x.ReceiverEmail)
            .NotEmpty()
            .WithMessage("Notification receiver email is required.")
            .EmailAddress()
            .WithMessage("{PropertyValue} is not a valid email address");

        RuleFor(x => x.TeamId.Value)
            .NotEmpty()
            .WithMessage("Team ID is required.");
    }
}