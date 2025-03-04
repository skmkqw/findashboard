using ZBank.Domain.UserAggregate.ValueObjects;

namespace ZBank.Application.Common.Interfaces.Services;

public interface IUserConnectionManager
{
    void AddConnection(UserId userId, string connectionId);

    void RemoveConnection(string connectionId);

    void SetActiveTeam(UserId userId, string teamId);
    
    void RemoveActiveTeam(UserId userId);
    
    List<string>? GetConnections(UserId userId);

    string? GetActiveTeam(UserId userId);
}