using ZBank.Domain.TeamAggregate;
using ZBank.Domain.TeamAggregate.ValueObjects;

namespace ZBank.Application.Common.Interfaces.Persistance;

public interface ISpaceRepository
{
    Task<PersonalSpace?> GetByIdAsync(TeamId id);
    
    void Add(PersonalSpace space);
}