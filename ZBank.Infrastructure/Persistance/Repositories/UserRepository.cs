using Microsoft.EntityFrameworkCore;
using ZBank.Application.Common.Interfaces.Persistance;
using ZBank.Domain.UserAggregate;

namespace ZBank.Infrastructure.Persistance.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ZBankDbContext _dbContext;

    public UserRepository(ZBankDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<User?> FindByEmailAsync(string email)
    {
        return await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == email);
    }

    public void Add(User user)
    {
        _dbContext.Users.Add(user);
    }
}