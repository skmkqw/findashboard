using ZBank.Domain.Common.Models;
using ZBank.Domain.ProfileAggregate.ValueObjects;
using ZBank.Domain.TeamAggregate.ValueObjects;
using ZBank.Domain.UserAggregate.ValueObjects;

namespace ZBank.Domain.UserAggregate;

public sealed class User : AggregateRoot<UserId>
{
    public string FirstName { get; }

    public string LastName { get; }

    public string Email { get; }

    public string Password { get; }
    
    public IReadOnlyList<TeamId> TeamIds => _teamIds.AsReadOnly();
    
    public IReadOnlyList<ProfileId> ProfileIds => _profileIds.AsReadOnly();
    
    private readonly List<TeamId> _teamIds = new();
    
    private readonly List<ProfileId> _profileIds = new();
    
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