using System.Collections.Concurrent;
using ErrorOr;
using ZBank.Application.Common.Interfaces.Persistance;
using ZBank.Application.Common.Interfaces.Services;
using ZBank.Domain.Common.Errors;
using ZBank.Domain.TeamAggregate.ValueObjects;
using ZBank.Domain.UserAggregate.ValueObjects;

namespace ZBank.Infrastructure.Services;

public class GroupManager : IGroupManager
{
    private readonly IUserRepository _userRepository;
    
    private readonly ITeamRepository _teamRepository;
    
    private readonly ConcurrentDictionary<string, ConcurrentDictionary<UserId, HashSet<string>>> _groups = new();

    public GroupManager(ITeamRepository teamRepository, IUserRepository userRepository)
    {
        _teamRepository = teamRepository;
        _userRepository = userRepository;
    }
    
    public async Task<ErrorOr<Success>> TryAddUserToGroupAsync(UserId userId, string connectionId, string groupId)
    {
        var validation = await ValidateGroupAsync(userId, groupId);
        if (validation.IsError) return validation;

        AddUserToGroup(userId, connectionId, groupId);
        return Result.Success;
    }

    public void RemoveUserFromGroup(UserId userId, string connectionId, string groupId)
    {
        if (_groups.TryGetValue(groupId, out var userConnections) &&
            userConnections.TryGetValue(userId, out var connections))
        {
            lock (connections)
            {
                connections.Remove(connectionId);
                if (connections.Count == 0)
                {
                    userConnections.TryRemove(userId, out _);
                }
            }

            if (userConnections.Count == 0)
            {
                _groups.TryRemove(groupId, out _);
            }
        }
    }

    public List<string>? GetConnectionsInGroup(string groupId)
    {
        if (_groups.TryGetValue(groupId, out var userConnections))
        {
            return userConnections.Values.SelectMany(connections => connections).ToList();
        }

        return null;
    }

    public List<UserId>? GetUsersInGroup(string groupId)
    {
        if (_groups.TryGetValue(groupId, out var userConnections))
        {
            return userConnections.Keys.ToList();
        }

        return null;
    }

    public List<string> GetAllGroups()
    {
        return _groups.Keys.ToList();
    }

    public bool IsUserInGroup(UserId userId, string groupId)
    {
        return _groups.TryGetValue(groupId, out var userConnections) && userConnections.ContainsKey(userId);
    }
    
    private void AddUserToGroup(UserId userId, string connectionId, string groupId)
    {
        var userConnections = _groups.GetOrAdd(groupId, _ => new ConcurrentDictionary<UserId, HashSet<string>>());
        var connections = userConnections.GetOrAdd(userId, _ => new HashSet<string>());

        lock (connections)
        {
            connections.Add(connectionId);
        }
    }
    
    private async Task<ErrorOr<Success>> ValidateGroupAsync(UserId userId, string groupId)
    {
        if (Guid.TryParse(groupId, out var validGroupId))
        {
            var teamId = TeamId.Create(validGroupId);
            
            var user = await _userRepository.FindByIdAsync(userId);
            if (user is null)
            {
                return Errors.User.IdNotFound(userId);
            }
            
            var teamValidationDetails = await _teamRepository.GetTeamValidationDetailsAsync(teamId, user);
            if (teamValidationDetails is null)
            {
                return Error.NotFound("Group.NotFound", "Team or personal space with given id not found does not exist");
            }

            if (!teamValidationDetails.HasAccess)
            {
                return Errors.Team.AccessDenied;
            }
            
            return Result.Success;
        }

        return Error.Validation("Group.InvalidIdFormat", "Group id has an invalid format");
    }
}