namespace ZBank.Application.Common.Interfaces.Persistance;

public interface IUnitOfWork
{
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}