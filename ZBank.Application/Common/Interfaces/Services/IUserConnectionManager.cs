using ZBank.Domain.UserAggregate.ValueObjects;

namespace ZBank.Application.Common.Interfaces.Services;

public interface IUserConnectionManager
{
    void AddConnection(UserId userId, string connectionId);

    void RemoveConnection(string connectionId);

    List<string>? GetConnections(UserId userId);
}