using ZBank.Domain.UserAggregate;
using ZBank.Domain.UserAggregate.ValueObjects;

namespace ZBank.Application.Common.Interfaces.Persistance;

public interface IUserRepository
{
    Task<User?> FindByIdAsync(UserId id);
    
    Task<User?> FindByEmailAsync(string email);
    
    void Add(User user);
}