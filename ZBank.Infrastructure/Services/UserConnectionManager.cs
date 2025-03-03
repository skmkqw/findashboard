using ZBank.Application.Common.Interfaces.Services;
using ZBank.Domain.UserAggregate.ValueObjects;

namespace ZBank.Infrastructure.Services;

public class UserConnectionManager : IUserConnectionManager
{
    private readonly Dictionary<UserId, List<string>> _userConnections = new();

    public void AddConnection(UserId userId, string connectionId)
    {
        lock (_userConnections)
        {
            if (!_userConnections.ContainsKey(userId))
            {
                _userConnections[userId] = new List<string>();
            }
            _userConnections[userId].Add(connectionId);
        }
    }

    public void RemoveConnection(string connectionId)
    {
        lock (_userConnections)
        {
            foreach (var userId in _userConnections.Keys)
            {
                if (_userConnections[userId].Remove(connectionId) && _userConnections[userId].Count == 0)
                {
                    _userConnections.Remove(userId);
                    break;
                }
            }
        }
    }

    public List<string>? GetConnections(UserId userId)
    {
        lock (_userConnections)
        {
            return _userConnections.TryGetValue(userId, out var connection) ? connection : null;
        }
    }
}
