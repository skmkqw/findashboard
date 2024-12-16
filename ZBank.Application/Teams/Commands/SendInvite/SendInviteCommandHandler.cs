using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;
using ZBank.Application.Common.Interfaces.Persistance;
using ZBank.Application.Common.Models;
using ZBank.Domain.Common.Errors;
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
        
        var sender = await _userRepository.FindByIdAsync(request.SenderId);

        if (sender is null)
        {
            _logger.LogInformation("User with ID: {Id} not found", request.SenderId.Value);

            return Errors.User.IdNotFound(request.SenderId);
        }
        
        var receiver = await _userRepository.FindByEmailAsync(request.ReceiverEmail);

        if (receiver is null)
        {
            _logger.LogInformation("User with email: {Email} not found", request.ReceiverEmail);

            return Errors.User.EmailNotFound(request.ReceiverEmail);
        }
        
        var team = await _teamRepository.GetByIdAsync(request.TeamId);

        if (team is null)
        {
            _logger.LogInformation("Team with id: {TeamId} not found", request.TeamId.Value);
            return Errors.Team.NotFound;
        }

        if (!team.UserIds.Contains(sender.Id))
        {
            _logger.LogInformation("Sender with email: {SenderEmail} is not a team member", sender.Email);
            return Errors.Team.MemberNotExists(sender.Email);
        }

        if (team.UserIds.Contains(receiver.Id))
        {
            _logger.LogInformation("User with email: {Email} is already in team", request.ReceiverEmail);
            return Errors.Team.MemberAlreadyExists(receiver.Email);
        }

        var existingInvite = await _notificationRepository.FindTeamInviteNotification(receiver.Id, team.Id);
        
        if (existingInvite is not null && receiver.NotificationIds.Contains(existingInvite.Id))
        {
            _logger.LogInformation("User with email: {ReceiverEmail} is already invited to team {TeamName}", receiver.Email, team.Name);
            return Errors.Notification.TeamInvite.TeamInviteAlreadyExists(receiver.Email, team.Name);
        }
        
        var teamInvite = NotificationFactory.CreateTeamInviteNotification(
            notificationSender: NotificationSender.Create(request.SenderId, string.Join(" ", sender.FirstName, sender.LastName)),
            receiverId: receiver.Id,
            teamId: request.TeamId,
            teamName: team.Name
        );
        
        _notificationRepository.AddNotification(teamInvite);
        
        receiver.AddNotificationId(teamInvite.Id);
        
        var inviteCreatedNotification = CreateInviteCreatedNotification(sender, receiver, team);
        _logger.LogInformation("'InviteSent' notification created");


        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        _logger.LogInformation("Successfully created and sent team invite");
        
        return new WithNotificationResult<TeamInviteNotification, InformationNotification>(teamInvite, inviteCreatedNotification);
    }

    private InformationNotification CreateInviteCreatedNotification(User inviteSender, User inviteReceiver, Team team)
    {
        var notification = NotificationFactory.CreateTemInviteSentNotification(inviteSender, inviteReceiver, team);
        
        _notificationRepository.AddNotification(notification);
        
        inviteSender.AddNotificationId(notification.Id);
        
        return notification;
    }
}