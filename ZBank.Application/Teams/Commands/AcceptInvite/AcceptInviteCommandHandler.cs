using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;
using ZBank.Application.Common.Interfaces.Persistance;
using ZBank.Domain.Common.Errors;
using ZBank.Domain.NotificationAggregate.Factories;
using ZBank.Domain.TeamAggregate;
using ZBank.Domain.UserAggregate;

namespace ZBank.Application.Teams.Commands.AcceptInvite;

public class AcceptInviteCommandHandler : IRequestHandler<AcceptInviteCommand, ErrorOr<Unit>>
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

    public async Task<ErrorOr<Unit>> Handle(AcceptInviteCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling team invite acceptation. Invite id: {InviteId}", request.NotificationId.Value);
        
        var invite = await _notificationRepository.FindTeamInviteNotificationById(request.NotificationId);

        if (invite is null)
        {
            _logger.LogInformation("Team invite with id: {Id} not found", request.NotificationId.Value);
            return Errors.Notification.TeamInvite.TeamInviteNotFound(request.NotificationId);
        }
        
        var inviteReceiver = await _userRepository.FindByIdAsync(request.UserId);
        var inviteSender = await _userRepository.FindByIdAsync(invite.NotificationSender.SenderId);

        if (inviteReceiver is null)
        {
            _logger.LogInformation("User with id: {Id} not found", request.UserId.Value);
            return Errors.User.IdNotFound(request.UserId);
        }

        if (inviteSender is null)
        {
            _logger.LogInformation("User with id: {Id} not found", request.UserId.Value);
            return Errors.User.IdNotFound(request.UserId);
        }

        if (invite.NotificationReceiverId != inviteReceiver.Id)
        {
            _logger.LogInformation("User with id: {Id} hasn't accepted invite since his not the receiver", invite.Id.Value);
            return Errors.Notification.TeamInvite.AccessDenied;
        }
        
        var team = await _teamRepository.GetByIdAsync(invite.TeamId);

        if (team is null)
        {
            _logger.LogInformation("Team with id: {TeamId} not found", invite.TeamId.Value);
            return Errors.Team.NotFound;
        }

        if (team.UserIds.Contains(inviteReceiver.Id))
        {
            _logger.LogInformation("User with id: {ReceiverId} is already a member of this team", inviteReceiver.Id.Value);
            return Errors.Team.MemberAlreadyExists(inviteReceiver.Email);
        }
        
        team.AddUserId(inviteReceiver.Id);
        
        inviteReceiver.AddTeamId(team.Id);
        
        _notificationRepository.DeleteTeamInviteNotification(invite);
        
        inviteReceiver.DeleteNotificationId(invite.Id);
        
        SendInviteAcceptedNotification(inviteSender, inviteReceiver, team);
        
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        _logger.LogInformation("Successfully accepted team invite");
        
        return Unit.Value;
    }
    
    private void SendInviteAcceptedNotification(User inviteSender, User inviteReceiver, Team team)
    {
        var notification = NotificationFactory.CreateTemInviteAcceptedNotification(inviteSender, inviteReceiver, team);
        
        _notificationRepository.AddInformationalNotification(notification);
        
        inviteSender.AddNotificationId(notification.Id);
    }
}