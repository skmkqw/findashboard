using FluentValidation;
using ZBank.Domain.NotificationAggregate.ValueObjects;

namespace ZBank.Application.Common.Validators;

public class NotificationSenderValidator : AbstractValidator<NotificationSender>
{
    public NotificationSenderValidator()
    {
        RuleFor(x => x.SenderId.Value)
            .NotEmpty()
            .WithMessage("Sender ID is required");
        
        RuleFor(x => x.SenderFullName)
            .NotEmpty()
            .WithMessage("Sender full name is required");
    }
}