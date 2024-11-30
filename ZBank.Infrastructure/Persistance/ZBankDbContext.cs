using Microsoft.EntityFrameworkCore;
using ZBank.Domain.ActivityAggregate;
using ZBank.Domain.Common.Attributes;
using ZBank.Domain.Common.Models;
using ZBank.Domain.NotificationAggregate;
using ZBank.Domain.ProfileAggregate;
using ZBank.Domain.ProjectAggregate;
using ZBank.Domain.TeamAggregate;
using ZBank.Domain.UserAggregate;
using ZBank.Domain.WalletAggregate;
using ZBank.Infrastructure.Persistance.Extensions;

namespace ZBank.Infrastructure.Persistance;

public class ZBankDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    
    public DbSet<Activity> Activities { get; set; }
    
    public DbSet<Profile> Profiles { get; set; }
    
    public DbSet<Project> Projects { get; set; }
    
    public DbSet<Team> Teams { get; set; }
    
    public DbSet<Wallet> Wallets { get; set; }
    
    public DbSet<Notification> Notifications { get; set; }
    
    public ZBankDbContext(DbContextOptions<ZBankDbContext> options) : base(options)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ZBankDbContext).Assembly);
        
        modelBuilder.AddStronglyTypedIdValueConverters<EfCoreValueConverterAttribute>();
    }
}