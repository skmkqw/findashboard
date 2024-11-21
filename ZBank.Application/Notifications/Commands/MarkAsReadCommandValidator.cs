using FluentValidation;

namespace ZBank.Application.Notifications.Commands;

public class MarkAsReadCommandValidator : AbstractValidator<MarkAsReadCommand>
{
    public MarkAsReadCommandValidator()
    {
        RuleFor(x => x.UserId.Value)
            .NotEmpty()
            .WithMessage("User ID is required.");
        
        RuleFor(x => x.NotificationId.Value)
            .NotEmpty()
            .WithMessage("Notification ID is required.");
    }
}