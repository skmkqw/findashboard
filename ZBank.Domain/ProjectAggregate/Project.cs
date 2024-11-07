using ZBank.Domain.Common.Models;
using ZBank.Domain.ProjectAggregate.ValueObjects;
using ZBank.Domain.TeamAggregate.ValueObjects;

namespace ZBank.Domain.ProjectAggregate;

public class Project : AggregateRoot<ProjectId>
{
    public string Name { get; }

    public TeamId TeamId { get; }

    private Project(string name, TeamId teamId)
    {
        Name = name;
        TeamId = teamId;
    }

    public static Project Create(string name, TeamId teamId)
    {
        return new Project(name, teamId);
    }
    
#pragma warning disable CS8618
    private Project()
#pragma warning restore CS8618
    {
    }
}