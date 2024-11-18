using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;
using ZBank.Application.Common.Interfaces.Persistance;
using ZBank.Domain.Common.Errors;
using ZBank.Domain.NotificationAggregate.Factories;

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
        _logger.LogInformation("Handling team invite creation from {SenderId} {TeamName} to {ReceiverEmail}", request.Sender.SenderId.Value, request.TeamName, request.ReceiverEmail);
        
        var member = await _userRepository.FindByEmailAsync(request.ReceiverEmail);

        if (member is not null)
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

        if (team.UserIds.Contains(member!.Id))
        {
            _logger.LogInformation("User with email: {Email} is already in team", request.ReceiverEmail);
            return Errors.Team.MemberExists(member.Email);
        }
        
        var teamInvite = NotificationFactory.CreateTeamInviteNotification(
            notificationSender: request.Sender,
            receiverId: member!.Id,
            teamId: request.TeamId,
            teamName: request.TeamName
        );
        
        _notificationRepository.AddTeamInvite(teamInvite);
        
        member.AddNotificationId(teamInvite.Id);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
        
        _logger.LogInformation("Successfully created and sent team invite");
        
        return Unit.Value;
    }
}