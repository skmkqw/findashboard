using Microsoft.EntityFrameworkCore;
using ZBank.Domain.UserAggregate;

namespace ZBank.Infrastructure.Persistance;

public class ZBankDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    
    public ZBankDbContext(DbContextOptions<ZBankDbContext> options) : base(options)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ZBankDbContext).Assembly);
        
        base.OnModelCreating(modelBuilder);
    }
}