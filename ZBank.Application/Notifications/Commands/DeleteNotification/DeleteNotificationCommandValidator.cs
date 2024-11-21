using FluentValidation;

namespace ZBank.Application.Notifications.Commands.DeleteNotification;

public class DeleteNotificationCommandValidator : AbstractValidator<DeleteNotificationCommand>
{
    public DeleteNotificationCommandValidator()
    {
        RuleFor(x => x.UserId.Value)
            .NotEmpty()
            .WithMessage("User ID is required.");
        
        RuleFor(x => x.NotificationId.Value)
            .NotEmpty()
            .WithMessage("Notification ID is required.");
    }
}