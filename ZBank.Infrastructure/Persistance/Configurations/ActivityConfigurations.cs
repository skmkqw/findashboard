using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ZBank.Domain.ActivityAggregate;
using ZBank.Domain.ActivityAggregate.ValueObjects;

namespace ZBank.Infrastructure.Persistance.Configurations;

public class ActivityConfigurations : IEntityTypeConfiguration<Activity>
{
    public void Configure(EntityTypeBuilder<Activity> builder)
    {
        ConfigureActivitiesTable(builder);
    }

    private void ConfigureActivitiesTable(EntityTypeBuilder<Activity> builder)
    {
        builder.ToTable("Activities");
        
        //PK
        builder.HasKey(x => x.Id);
        
        //ID
        builder.Property(x => x.Id)
            .ValueGeneratedOnAdd()
            .HasConversion(
                id => id.Value,
                value => ActivityId.Create(value));
        
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
            aib.HasKey("Id");
            
            //ID
            aib.Property(x => x.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("ActivityLogId")
                .HasConversion(
                    id => id.Value,
                    value => ActivityLogId.Create(value));
            
            //Timestamp
            aib.Property(a => a.TimeStamp)
                .ValueGeneratedNever();
            
        });
        
        builder.Metadata.FindNavigation(nameof(Activity.ActivityLogs))!
            .SetPropertyAccessMode(PropertyAccessMode.Field);
    }
}