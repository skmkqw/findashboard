using ZBank.Application.Authentication.Common;

namespace ZBank.Application.Common.Interfaces.Persistance;

public interface IUserRepository
{
    Task<User?> FindByEmailAsync(string email);
    void Add(User user);
}