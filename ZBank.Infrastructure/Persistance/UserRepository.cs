using ZBank.Application.Authentication.Common;
using ZBank.Application.Common.Interfaces.Persistance;

namespace ZBank.Infrastructure.Persistance;

public class UserRepository : IUserRepository
{
    private static readonly List<User> users = new();

    public async Task<User?> FindByEmailAsync(string email)
    {
        await Task.CompletedTask;
        return users.Find(u => u.Email == email);
    }

    public void Add(User user)
    {
        users.Add(user);
    }
}