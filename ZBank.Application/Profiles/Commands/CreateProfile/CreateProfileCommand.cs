using ErrorOr;
using MediatR;
using ZBank.Application.Common.Models;
using ZBank.Domain.NotificationAggregate;
using ZBank.Domain.ProfileAggregate;
using ZBank.Domain.TeamAggregate.ValueObjects;
using ZBank.Domain.UserAggregate.ValueObjects;

namespace ZBank.Application.Profiles.Commands.CreateProfile;

public record CreateProfileCommand(string Name, UserId OwnerId, TeamId TeamId) : IRequest<ErrorOr<WithNotificationResult<Profile, InformationNotification>>>;