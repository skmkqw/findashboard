using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ZBank.Domain.ActivityAggregate;

namespace ZBank.Infrastructure.Persistance.Configurations;

public class ActivityConfigurations : IEntityTypeConfiguration<Activity>
{
    public void Configure(EntityTypeBuilder<Activity> builder)
    {
        ConfigureActivitiesTable(builder);
        ConfigureActivityLogsTable(builder);
    }

    private void ConfigureActivitiesTable(EntityTypeBuilder<Activity> builder)
    {
        builder.ToTable("Activities");
        
        //PK
        builder.HasKey(x => x.Id);
        
        //ID
        builder.Property(x => x.Id)
            .ValueGeneratedNever();
        
        //TeamId
        builder.Property(x => x.TeamId)
            .ValueGeneratedNever();
        
        //ProjectId
        builder.Property(x => x.ProjectId)
            .ValueGeneratedNever();
        
        //Name
        builder.Property(x => x.Name)
            .HasMaxLength(100);
    }

    private void ConfigureActivityLogsTable(EntityTypeBuilder<Activity> builder)
    {
        builder.OwnsMany(a => a.ActivityLogs, aib =>
        {
            aib.ToTable("ActivityLogs");
            
            //FK
            aib.WithOwner().HasForeignKey("ActivityId");
            
            //PK
            aib.HasKey("Id", "ActivityId");
            
            //ID
            aib.Property(x => x.Id)
                .ValueGeneratedNever();
            
            //UserId
            aib.Property(x => x.UserId)
                .ValueGeneratedNever();
            
            //ProfileId
            aib.Property(x => x.ProfileId)
                .ValueGeneratedNever();
            
            //WalletId
            aib.Property(x => x.WalletId)
                .ValueGeneratedNever();
            
            //Timestamp
            aib.Property(a => a.TimeStamp)
                .ValueGeneratedNever();
            
        });
        
        builder.Metadata.FindNavigation(nameof(Activity.ActivityLogs))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);
    }
}