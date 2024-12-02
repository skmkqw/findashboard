using FluentValidation;

namespace ZBank.Application.Spaces.Queries.GetSpace;

public class GetSpaceQueryValidator : AbstractValidator<GetSpaceQuery>
{
    public GetSpaceQueryValidator()
    {
        RuleFor(x => x.OwnerId.Value)
            .NotEmpty()
            .WithMessage("Owner ID is required.");
    }
}