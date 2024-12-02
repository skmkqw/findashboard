using ErrorOr;
using MediatR;
using ZBank.Domain.TeamAggregate;
using ZBank.Domain.UserAggregate.ValueObjects;

namespace ZBank.Application.Spaces.Queries.GetSpace;

public record GetSpaceQuery(UserId OwnerId) : IRequest<ErrorOr<PersonalSpace>>;