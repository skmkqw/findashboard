using System.Collections.Concurrent;
using ZBank.Application.Common.Interfaces.Services;
using ZBank.Domain.UserAggregate.ValueObjects;

namespace ZBank.Infrastructure.Services;

public class GroupManager : IGroupManager
{
    private readonly ConcurrentDictionary<string, ConcurrentDictionary<UserId, HashSet<string>>> _groups = new();

    public void AddUserToGroup(UserId userId, string connectionId, string groupId)
    {
        var userConnections = _groups.GetOrAdd(groupId, _ => new ConcurrentDictionary<UserId, HashSet<string>>());
        var connections = userConnections.GetOrAdd(userId, _ => new HashSet<string>());
        
        lock (connections)
        {
            connections.Add(connectionId);
        }
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
}
