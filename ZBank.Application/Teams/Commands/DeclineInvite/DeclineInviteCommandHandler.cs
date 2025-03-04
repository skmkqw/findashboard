using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;
using ZBank.Application.Common.Interfaces.Persistance;
using ZBank.Application.Common.Models;
using ZBank.Application.Common.Models.Validation;
using ZBank.Application.Teams.Commands.AcceptInvite;
using ZBank.Domain.Common.Errors;
using ZBank.Domain.NotificationAggregate;
using ZBank.Domain.NotificationAggregate.Factories;
using ZBank.Domain.TeamAggregate;
using ZBank.Domain.UserAggregate;

namespace ZBank.Application.Teams.Commands.DeclineInvite;

public class DeclineInviteCommandHandler : IRequestHandler<DeclineInviteCommand, ErrorOr<WithNotificationResult<Unit, InformationNotification>>>
{
    private readonly IUserRepository _userRepository;
    
    private readonly INotificationRepository _notificationRepository;
    
    private readonly ITeamRepository _teamRepository;
    
    private readonly IUnitOfWork _unitOfWork;
    
    private readonly ILogger<DeclineInviteCommandHandler> _logger;

    public DeclineInviteCommandHandler(IUserRepository userRepository,
        INotificationRepository notificationRepository,
        ITeamRepository teamRepository,
        IUnitOfWork unitOfWork,
        ILogger<DeclineInviteCommandHandler> logger)
    {
        _userRepository = userRepository;
        _notificationRepository = notificationRepository;
        _teamRepository = teamRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<ErrorOr<WithNotificationResult<Unit, InformationNotification>>> Handle(DeclineInviteCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling team invite declination. Invite id: {InviteId}", request.NotificationId.Value);
        
        var invite = await _notificationRepository.FindNotificationById<TeamInviteNotification>(request.NotificationId);

        if (invite is null)
        {
            _logger.LogInformation("Team invite with id: {Id} not found", request.NotificationId.Value);
            return Errors.Notification.TeamInvite.NotFound(request.NotificationId);
        }
        
        var (senderResult, receiverResult) = await FindSenderAndReceiverAsync(invite);
        if (senderResult.IsError) return senderResult.Errors;
        if (receiverResult.IsError) return receiverResult.Errors;

        var sender = senderResult.Value;
        var receiver = receiverResult.Value;
        
        var teamValidationDetails = await _teamRepository.GetTeamValidationDetailsAsync(invite.TeamId, receiver);
        
        if (teamValidationDetails is null)
        {
            _logger.LogInformation("Team with id: {TeamId} not found", invite.TeamId.Value);
            return Errors.Team.NotFound;
        }
        
        if (ValidateDeclineInvite(invite, receiver, teamValidationDetails) is var validationResult && validationResult.IsError)
            return validationResult.Errors;
        
        var team = teamValidationDetails.GetTeamOrSpace() as Team;

        DeclineInvite(invite, receiver);
        _logger.LogInformation("Successfully declined team invite");
        
        var inviteDeclinedNotification = CreateInviteDeclinedNotification(sender, receiver, team);
        _logger.LogInformation("'InviteDeclined' notification created");
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return new WithNotificationResult<Unit, InformationNotification>(Unit.Value, inviteDeclinedNotification);
    }
    
    private void DeclineInvite(TeamInviteNotification invite, User receiver)
    {
        _notificationRepository.DeleteNotification(invite);
        
        receiver.DeleteNotificationId(invite.Id);
    }
    
    private ErrorOr<Success> ValidateDeclineInvite(TeamInviteNotification invite, User receiver, TeamValidationDetails teamValidationDetails)
    {
        if (!teamValidationDetails.IsTeam)
        {
            _logger.LogInformation("Blocked attempt to accept invite to personal space with Id: {PersonalSpaceId}", invite.TeamId);
            return Errors.PersonalSpace.InvalidOperation;
        }
        
        if (invite.NotificationReceiverId != receiver.Id)
        {
            _logger.LogInformation("User with id: {Id} can't accept invite since he is not the receiver", invite.Id.Value);
            return Errors.Notification.TeamInvite.AccessDenied;
        }
        
        if (teamValidationDetails.HasAccess)
        {
            _logger.LogInformation("User with id: {ReceiverId} is already a member of this team", receiver.Id.Value);
            return Errors.Team.MemberAlreadyExists(receiver.Email);
        }
        
        return Result.Success;
    }
    
    private async Task<(ErrorOr<User> Sender, ErrorOr<User> Receiver)> FindSenderAndReceiverAsync(TeamInviteNotification invite)
    {
        var sender = await _userRepository.FindByIdAsync(invite.NotificationSender.SenderId);
        if (sender is null)
        {
            _logger.LogInformation("Notification sender with ID: {Id} not found", invite.NotificationSender.SenderId.Value);
            return (Errors.User.IdNotFound(invite.NotificationSender.SenderId), new ErrorOr<User>());
        }

        var receiver = await _userRepository.FindByIdAsync(invite.NotificationSender.SenderId);
        if (receiver is null)
        {
            _logger.LogInformation("Notification receiver with ID: {Id} not found", invite.NotificationSender.SenderId.Value);
            return (Errors.User.IdNotFound(invite.NotificationSender.SenderId), new ErrorOr<User>());
        }

        return (sender, receiver);
    }
    
    private InformationNotification CreateInviteDeclinedNotification(User inviteSender, User inviteReceiver, Team team)
    {
        var notification = NotificationFactory.CreateTemInviteDeclinedNotification(inviteSender, inviteReceiver, team);
        
        _notificationRepository.AddNotification(notification);
        
        inviteSender.AddNotificationId(notification.Id);
        
        return notification;
    }
}