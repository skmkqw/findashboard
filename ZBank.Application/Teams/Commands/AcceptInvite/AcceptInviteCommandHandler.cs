using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;
using ZBank.Application.Common.Interfaces.Persistance;
using ZBank.Application.Common.Models;
using ZBank.Application.Common.Models.Validation;
using ZBank.Domain.Common.Errors;
using ZBank.Domain.NotificationAggregate;
using ZBank.Domain.NotificationAggregate.Factories;
using ZBank.Domain.TeamAggregate;
using ZBank.Domain.UserAggregate;

namespace ZBank.Application.Teams.Commands.AcceptInvite;

public class AcceptInviteCommandHandler : IRequestHandler<AcceptInviteCommand, ErrorOr<WithNotificationResult<Success, InformationNotification>>>
{
    private readonly IUserRepository _userRepository;
    
    private readonly INotificationRepository _notificationRepository;
    
    private readonly ITeamRepository _teamRepository;
    
    private readonly IUnitOfWork _unitOfWork;
    
    private readonly ILogger<AcceptInviteCommandHandler> _logger;

    public AcceptInviteCommandHandler(IUserRepository userRepository,
        INotificationRepository notificationRepository,
        ITeamRepository teamRepository,
        IUnitOfWork unitOfWork,
        ILogger<AcceptInviteCommandHandler> logger)
    {
        _userRepository = userRepository;
        _notificationRepository = notificationRepository;
        _teamRepository = teamRepository;
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<ErrorOr<WithNotificationResult<Success, InformationNotification>>> Handle(AcceptInviteCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling team invite acceptation. Invite id: {InviteId}", request.NotificationId.Value);
        
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
        
        var acceptInviteValidationResult = ValidateAcceptInvite(invite, receiver, teamValidationDetails);
        
        if (acceptInviteValidationResult.IsError)
            return acceptInviteValidationResult.Errors;
        
        var team = teamValidationDetails.GetTeamOrSpace() as Team;
        
        AcceptInvite(invite, receiver, team);
        _logger.LogInformation("Successfully accepted team invite");

        var inviteAcceptedNotification = CreateInviteAcceptedNotification(sender, receiver, team);
        _logger.LogInformation("'InviteAccepted' notification created");
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return new WithNotificationResult<Success, InformationNotification>(Result.Success, inviteAcceptedNotification);
    }
    
    private void AcceptInvite(TeamInviteNotification invite, User receiver, Team team)
    {
        team.AddUserId(receiver.Id);
        
        receiver.AddTeamId(team.Id);
        
        _notificationRepository.DeleteNotification(invite);
        
        receiver.DeleteNotificationId(invite.Id);
    }

    private ErrorOr<Success> ValidateAcceptInvite(TeamInviteNotification invite, User receiver, TeamValidationDetails teamValidationDetails)
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
        var sender = await _userRepository.FindByIdAsync(invite.NotificationSenderId);
        if (sender is null)
        {
            _logger.LogInformation("Notification sender with ID: {Id} not found", invite.NotificationSenderId.Value);
            return (Errors.User.IdNotFound(invite.NotificationSenderId), new ErrorOr<User>());
        }

        var receiver = await _userRepository.FindByIdAsync(invite.NotificationSenderId);
        if (receiver is null)
        {
            _logger.LogInformation("Notification receiver with ID: {Id} not found", invite.NotificationSenderId.Value);
            return (Errors.User.IdNotFound(invite.NotificationSenderId), new ErrorOr<User>());
        }

        return (sender, receiver);
    }
    private InformationNotification CreateInviteAcceptedNotification(User inviteSender, User inviteReceiver, Team team)
    {
        var notification = NotificationFactory.CreateTemInviteAcceptedNotification(inviteSender, inviteReceiver, team);
        
        _notificationRepository.AddNotification(notification);
        
        inviteSender.AddNotificationId(notification.Id);
        
        return notification;
    }
}