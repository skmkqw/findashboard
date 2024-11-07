using ZBank.Domain.Common.Models;
using ZBank.Domain.UserAggregate.ValueObjects;

namespace ZBank.Domain.UserAggregate;

public sealed class User : AggregateRoot<UserId>
{
    public string FirstName { get; }

    public string LastName { get; }

    public string Email { get; }

    public string Password { get; }
    
    public IReadOnlyList<UserId> UserIds => _userIds.AsReadOnly();
    
    private readonly List<UserId> _userIds = new();
    
    private User(
        UserId id, 
        string firstName, 
        string lastName, 
        string email, 
        string password) : base(id)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Password = password;
    }

    public static User Create(string firstName, string lastName, string email, string password)
    {
        return new User(
            UserId.CreateUnique(), 
            firstName, lastName,
            email, 
            password);
    }
    
#pragma warning disable CS8618
    private User()
#pragma warning restore CS8618
    {
    }
}