using ErrorOr;
using MediatR;
using ZBank.Application.Common.Models;
using ZBank.Domain.NotificationAggregate;
using ZBank.Domain.TeamAggregate;
using ZBank.Domain.UserAggregate.ValueObjects;

namespace ZBank.Application.Spaces.Commands.CreateSpace;

public record CreateSpaceCommand(UserId OwnerId, string Name) : IRequest<ErrorOr<WithNotificationResult<PersonalSpace, InformationNotification>>>;