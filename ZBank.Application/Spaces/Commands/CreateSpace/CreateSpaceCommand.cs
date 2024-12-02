using ErrorOr;
using MediatR;
using ZBank.Domain.TeamAggregate;
using ZBank.Domain.UserAggregate.ValueObjects;

namespace ZBank.Application.Spaces.Commands.CreateSpace;

public record CreateSpaceCommand(UserId OwnerId, string Name) : IRequest<ErrorOr<PersonalSpace>>;