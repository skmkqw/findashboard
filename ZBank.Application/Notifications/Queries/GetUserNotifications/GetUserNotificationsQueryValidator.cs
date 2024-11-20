using FluentValidation;

namespace ZBank.Application.Notifications.Queries.GetUserNotifications;

public class GetUserNotificationsQueryValidator : AbstractValidator<GetUserNotificationsQuery>
{
    public GetUserNotificationsQueryValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("User Id is required");
    }
}