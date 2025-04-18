namespace ZBank.Domain.Common.Models;

public interface IEntityId<T, TValue> where T : IEntityId<T, TValue>
{
    TValue Value { get; }
    
    static abstract T Create(TValue value);
}
