using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;
using ZBank.Application.Common.Interfaces.Persistance;
using ZBank.Domain.Common.Errors;
using ZBank.Domain.NotificationAggregate.Factories;
using ZBank.Domain.NotificationAggregate.ValueObjects;

namespace ZBank.Application.Teams.Commands.SendInvite;

public class SendInviteCommandHandler : IRequestHandler<SendInviteCommand, ErrorOr<Unit>>
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

    public async Task<ErrorOr<Unit>> Handle(SendInviteCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling team invite creation from {SenderId} to {ReceiverEmail}. Team id: {TeamId}", request.SenderId.Value, request.ReceiverEmail, request.TeamId);
        
        var sender = await _userRepository.FindByIdAsync(request.SenderId);

        if (sender is null)
        {
            _logger.LogInformation("User with ID: {Id} not found", request.SenderId);

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
            _logger.LogInformation("Team with id: {TeamId} not found", request.TeamId);
            return Errors.Team.NotFound;
        }

        if (!team.UserIds.Contains(sender.Id))
        {
            _logger.LogInformation("Sender with email: {SenderEmail} is not a team member", sender.Email);
            return Errors.Team.MemberNotExists(sender.Email);
        }

        if (team.UserIds.Contains(receiver!.Id))
        {
            _logger.LogInformation("User with email: {Email} is already in team", request.ReceiverEmail);
            return Errors.Team.MemberAlreadyExists(receiver.Email);
        }
        
        var teamInvite = NotificationFactory.CreateTeamInviteNotification(
            notificationSender: NotificationSender.Create(request.SenderId, string.Join(" ", sender.FirstName, sender.LastName)),
            receiverId: receiver.Id,
            teamId: request.TeamId,
            teamName: team.Name
        );
        
        _notificationRepository.AddTeamInvite(teamInvite);
        
        receiver.AddNotificationId(teamInvite.Id);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        _logger.LogInformation("Successfully created and sent team invite");
        
        return Unit.Value;
    }
}