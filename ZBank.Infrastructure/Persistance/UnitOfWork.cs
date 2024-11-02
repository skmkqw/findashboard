using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using ZBank.Application.Common.Interfaces.Persistance;
using ZBank.Domain.Common.Models;

namespace ZBank.Infrastructure.Persistance;

public class UnitOfWork : IUnitOfWork
{
    private readonly ZBankDbContext _dbContext;

    public UnitOfWork(ZBankDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        UpdateTimestamps();
        
        await _dbContext.SaveChangesAsync(cancellationToken);
    }
    
    private void UpdateTimestamps()
    {
        IEnumerable<EntityEntry<IHasTimestamps>> entries = _dbContext
            .ChangeTracker
            .Entries<IHasTimestamps>();

        foreach (EntityEntry<IHasTimestamps> entityEntry in entries)
        {
            if (entityEntry.State == EntityState.Added)
            {
                entityEntry.Property(x => x.CreatedDateTime)
                    .CurrentValue = DateTime.UtcNow;
            }

            if (entityEntry.State == EntityState.Modified)
            {
                entityEntry.Property(x => x.UpdatedDateTime)
                    .CurrentValue = DateTime.UtcNow;
            }
        }
    }
}