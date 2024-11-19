using ZBank.Domain.Common.Models;
using ZBank.Domain.NotificationAggregate.ValueObjects;
using ZBank.Domain.ProfileAggregate.ValueObjects;
using ZBank.Domain.TeamAggregate.ValueObjects;
using ZBank.Domain.UserAggregate.ValueObjects;

namespace ZBank.Domain.UserAggregate;

public sealed class User : AggregateRoot<UserId>
{
    public string FirstName { get; private set;}

    public string LastName { get; private set;}

    public string Email { get; }

    public string Password { get; }
    
    public IReadOnlyList<TeamId> TeamIds => _teamIds.AsReadOnly();
    
    public IReadOnlyList<ProfileId> ProfileIds => _profileIds.AsReadOnly();
    
    public IReadOnlyList<NotificationId> NotificationIds => _notificationIds.AsReadOnly();
    
    private readonly List<TeamId> _teamIds = new();
    
    private readonly List<ProfileId> _profileIds = new();
    
    private readonly List<NotificationId> _notificationIds = new();
    
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

    public static User Create( 
        string firstName,
        string lastName, 
        string email,
        string password)
    {
        return new User(
            UserId.CreateUnique(), 
            firstName, lastName,
            email, 
            password);
    }

    public void Update(string firstName, string lastName)
    {
        FirstName = firstName;
        LastName = lastName;
    }

    public void AddTeamId(TeamId teamId) => _teamIds.Add(teamId);

    public void DeleteTeamId(TeamId teamId) => _teamIds.Remove(teamId);

    public void AddProfileId(ProfileId profileId) => _profileIds.Add(profileId);

    public void DeleteProfileId(ProfileId profileId) => _profileIds.Remove(profileId);
    
    public void AddNotificationId(NotificationId notificationId) => _notificationIds.Add(notificationId);

    public void DeleteNotificationId(NotificationId notificationId) => _notificationIds.Remove(notificationId);

#pragma warning disable CS8618
    private User()
#pragma warning restore CS8618
    {
    }
}