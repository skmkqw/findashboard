using ErrorOr;
using ZBank.Domain.UserAggregate.ValueObjects;

namespace ZBank.Application.Common.Interfaces.Services;

public interface IGroupManager
{
    Task<ErrorOr<Success>> TryAddUserToGroupAsync(UserId userId, string connectionId, string groupId);

    void RemoveUserFromGroup(UserId userId, string connectionId, string groupId);

    List<string>? GetConnectionsInGroup(string groupId);

    List<UserId>? GetUsersInGroup(string groupId);

    List<string> GetAllGroups();

    bool IsUserInGroup(UserId userId, string groupId);
}