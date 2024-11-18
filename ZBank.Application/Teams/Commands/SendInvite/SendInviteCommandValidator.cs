using FluentValidation;
using ZBank.Application.Common.Validators;

namespace ZBank.Application.Teams.Commands.SendInvite;

public class SendInviteCommandValidator : AbstractValidator<SendInviteCommand>
{
    public SendInviteCommandValidator()
    {
        RuleFor(x => x.Sender)
            .NotEmpty()
            .WithMessage("Notification sender is required.")
            .SetValidator(new NotificationSenderValidator());
        
        RuleFor(x => x.ReceiverEmail)
            .NotEmpty()
            .WithMessage("Notification receiver email is required.")
            .EmailAddress()
            .WithMessage("{PropertyValue} is not a valid email address");

        RuleFor(x => x.TeamId.Value)
            .NotEmpty()
            .WithMessage("Team ID is required.");
        
        RuleFor(x => x.TeamName)
            .NotEmpty()
            .WithMessage("Team name is required.");
    }
}