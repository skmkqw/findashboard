namespace ZBank.Application.Common.Interfaces.Services.Authentication;

public interface IPasswordHasher
{
    string HashPassword(string password);
    
    bool VerifyHashedPassword(string hashedPassword, string password);
}