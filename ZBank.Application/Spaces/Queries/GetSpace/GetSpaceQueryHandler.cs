using ErrorOr;
using MediatR;
using Microsoft.Extensions.Logging;
using ZBank.Application.Common.Interfaces.Persistance;
using ZBank.Application.Spaces.Commands.CreateSpace;
using ZBank.Domain.Common.Errors;
using ZBank.Domain.TeamAggregate;
using ZBank.Domain.UserAggregate;

namespace ZBank.Application.Spaces.Queries.GetSpace;

public class GetSpaceQueryHandler : IRequestHandler<GetSpaceQuery, ErrorOr<PersonalSpace>>
{
    private readonly IUserRepository _userRepository;
    
    private readonly ITeamRepository _teamRepository;
    
    private readonly ILogger<CreateSpaceCommandHandler> _logger;

    public GetSpaceQueryHandler(IUserRepository userRepository,
        ITeamRepository teamRepository,
        ILogger<CreateSpaceCommandHandler> logger)
    {
        _userRepository = userRepository;
        _teamRepository = teamRepository;
        _logger = logger;
    }

    public async Task<ErrorOr<PersonalSpace>> Handle(GetSpaceQuery request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Handling GetSpace query for user with id: {UserId}", request.OwnerId.Value);
        
        var owner = await _userRepository.FindByIdAsync(request.OwnerId);

        if (owner is null)
        {
            _logger.LogInformation("User with id: {UserId} not found or does not exist", request.OwnerId.Value);
            return Errors.User.IdNotFound(request.OwnerId);
        }
        
        if (ValidateGetSpace(owner) is var validationResult && validationResult.IsError)
            return validationResult.Errors;
        
        var space = await _teamRepository.GetSpaceByIdAsync(owner.PersonalSpaceId!);
        _logger.LogInformation("Successfully fetched personal space");

        return space!;
    }

    private ErrorOr<Success> ValidateGetSpace(User spaceOwner)
    {
        if (spaceOwner.PersonalSpaceId is null)
        {
            _logger.LogInformation("User with id: {UserId} has no personal space", spaceOwner.Id.Value);
            return Errors.PersonalSpace.IsNotSet;
        }

        return Result.Success;
    }
}