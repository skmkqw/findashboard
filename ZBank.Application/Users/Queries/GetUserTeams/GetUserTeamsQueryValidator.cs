using FluentValidation;

namespace ZBank.Application.Users.Queries.GetUserTeams;

public class GetUserTeamsQueryValidator : AbstractValidator<GetUserTeamsQuery>
{
    public GetUserTeamsQueryValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty()
            .WithMessage("User ID cannot be empty");
    }
}