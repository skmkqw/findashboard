using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;
using ZBank.Application.Common.Interfaces.Persistance;
using ZBank.Application.Common.Models;
using ZBank.Application.Common.Models.Validation;
using ZBank.Domain.Common.Errors;
using ZBank.Domain.Common.Models;
using ZBank.Domain.NotificationAggregate;
using ZBank.Domain.NotificationAggregate.Factories;
using ZBank.Domain.NotificationAggregate.ValueObjects;
using ZBank.Domain.TeamAggregate;
using ZBank.Domain.UserAggregate;

namespace ZBank.Application.Teams.Commands.SendInvite;

public class SendInviteCommandHandler : IRequestHandler<SendInviteCommand, ErrorOr<WithNotificationResult<TeamInviteNotification, InformationNotification>>>
{
    private readonly ILogger<SendInviteCommandHandler> _logger;
    
    private readonly IUserRepository _userRepository;
    
    private readonly ITeamRepository _teamRepository;
    
    private readonly INotificationRepository _notificationRepository;
    
    private readonly IUnitOfWork _unitOfWork;

    public SendInviteCommandHandler(ILogger<SendInviteCommandHandler> logger,
        IUserRepository userRepository,
        ITeamRepository teamRepository,
        INotificationRepository notificationRepository,
        IUnitOfWork unitOfWork)
    {
        _logger = logger;
        _userRepository = userRepository;
        _teamRepository = teamRepository;
        _notificationRepository = notificationRepository;
        _unitOfWork = unitOfWork;
    }
    
    public async Task<ErrorOr<WithNotificationResult<TeamInviteNotification, InformationNotification>>> Handle(SendInviteCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling team invite creation from {SenderId} to {ReceiverEmail}. Team id: {TeamId}", request.SenderId.Value, request.ReceiverEmail, request.TeamId.Value);
        
        var (senderResult, receiverResult) = await FindSenderAndReceiverAsync(request);
        if (senderResult.IsError) return senderResult.Errors;
        if (receiverResult.IsError) return receiverResult.Errors;

        var sender = senderResult.Value;
        var receiver = receiverResult.Value;
        
        var teamValidationDetails = await _teamRepository.GetTeamValidationDetailsAsync(request.TeamId, sender);

        if (teamValidationDetails is null)
        {
            _logger.LogInformation("Team with id: {TeamId} not found", request.TeamId.Value);
            return Errors.Team.NotFound;
        }
        
        var teamOrSpace = teamValidationDetails.GetTeamOrSpace();

        if (await ValidateSendInviteAsync(request, teamValidationDetails, receiver) is var validationResult && validationResult.IsError)
            return validationResult.Errors;
        
        var teamInvite = SendInvite(request, teamOrSpace as Team, sender, receiver);
        _logger.LogInformation("Successfully created team invite");

        var inviteCreatedNotification = CreateInviteCreatedNotification(sender, receiver, teamOrSpace as Team);
        _logger.LogInformation("'InviteSent' notification created");
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        return new WithNotificationResult<TeamInviteNotification, InformationNotification>(teamInvite, inviteCreatedNotification);
    }
    
    private async Task<(ErrorOr<User> Sender, ErrorOr<User> Receiver)> FindSenderAndReceiverAsync(SendInviteCommand request)
    {
        var sender = await _userRepository.FindByIdAsync(request.SenderId);
        if (sender is null)
        {
            _logger.LogInformation("Notification sender with ID: {Id} not found", request.SenderId.Value);
            return (Errors.User.IdNotFound(request.SenderId), new ErrorOr<User>());
        }

        var receiver = await _userRepository.FindByEmailAsync(request.ReceiverEmail);
        if (receiver is null)
        {
            _logger.LogInformation("Notification receiver with email: {Email} not found", request.ReceiverEmail);
            return (new ErrorOr<User>(), Errors.User.EmailNotFound(request.ReceiverEmail));
        }

        return (sender, receiver);
    }

    private async Task<ErrorOr<Success>> ValidateSendInviteAsync(SendInviteCommand request, TeamValidationDetails teamValidationDetails, User receiver)
    {
        (TeamBase teamOrSpace, User sender) = teamValidationDetails.GetEntities();
        
        if (!teamValidationDetails.IsTeam)
        {
            _logger.LogInformation("Blocked attempt to send invite to personal space with Id: {PersonalSpaceId}", request.TeamId);
            return Errors.PersonalSpace.InvalidOperation;
        }

        if (!teamValidationDetails.MemberHasAccess(sender.Id))
        {
            _logger.LogInformation("Sender with email: {SenderEmail} is not a team member", sender.Email);
            return Errors.Team.AccessDenied;
        }

        if (teamValidationDetails.MemberHasAccess(receiver.Id))
        {
            _logger.LogInformation("User with email: {Email} is already in team", request.ReceiverEmail);
            return Errors.Team.MemberAlreadyExists(receiver.Email);
        }

        var existingInvite = await _notificationRepository.FindTeamInviteNotification(receiver.Id, teamOrSpace.Id);
        
        if (existingInvite is not null && receiver.NotificationIds.Contains(existingInvite.Id))
        {
            _logger.LogInformation("User with email: {ReceiverEmail} is already invited to team {TeamName}", receiver.Email, teamOrSpace.Name);
            return Errors.Notification.TeamInvite.TeamInviteAlreadyExists(receiver.Email, teamOrSpace.Name);
        }

        return Result.Success;
    }

    private TeamInviteNotification SendInvite(SendInviteCommand request, Team team, User sender, User receiver)
    {
        var teamInvite = NotificationFactory.CreateTeamInviteNotification(
            notificationSender: NotificationSender.Create(request.SenderId, string.Join(" ", sender.FirstName, sender.LastName)),
            receiverId: receiver.Id,
            teamId: request.TeamId,
            teamName: team.Name
        );
        
        _notificationRepository.AddNotification(teamInvite);
        
        receiver.AddNotificationId(teamInvite.Id);
        
        return teamInvite;
    }

    private InformationNotification CreateInviteCreatedNotification(User inviteSender, User inviteReceiver, Team team)
    {
        var notification = NotificationFactory.CreateTemInviteSentNotification(inviteSender, inviteReceiver, team);
        
        _notificationRepository.AddNotification(notification);
        
        inviteSender.AddNotificationId(notification.Id);
        
        return notification;
    }
}